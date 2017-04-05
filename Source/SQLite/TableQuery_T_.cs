using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SQLite
{
	public class TableQuery<T> : BaseTableQuery, IEnumerable<T>, IEnumerable
	{
		private Expression _where;

		private List<BaseTableQuery.Ordering> _orderBys;

		private int? _limit;

		private int? _offset;

		private BaseTableQuery _joinInner;

		private Expression _joinInnerKeySelector;

		private BaseTableQuery _joinOuter;

		private Expression _joinOuterKeySelector;

		private Expression _joinSelector;

		private Expression _selector;

		private bool _deferred;

		public SQLiteConnection Connection
		{
			get;
			private set;
		}

		public TableMapping Table
		{
			get;
			private set;
		}

		private TableQuery(SQLiteConnection conn, TableMapping table)
		{
			this.Connection = conn;
			this.Table = table;
		}

		public TableQuery(SQLiteConnection conn)
		{
			this.Connection = conn;
			this.Table = this.Connection.GetMapping(typeof(T), CreateFlags.None);
		}

		private TableQuery<T> AddOrderBy<U>(Expression<Func<T, U>> orderExpr, bool asc)
		{
			if (orderExpr.NodeType != ExpressionType.Lambda)
			{
				throw new NotSupportedException("Must be a predicate");
			}
			LambdaExpression lambdaExpression = orderExpr;
			MemberExpression memberExpression = null;
			UnaryExpression body = lambdaExpression.Body as UnaryExpression;
			memberExpression = (body == null || body.NodeType != ExpressionType.Convert ? lambdaExpression.Body as MemberExpression : body.Operand as MemberExpression);
			if (memberExpression == null || memberExpression.Expression.NodeType != ExpressionType.Parameter)
			{
				throw new NotSupportedException(string.Concat("Order By does not support: ", orderExpr));
			}
			TableQuery<T> orderings = this.Clone<T>();
			if (orderings._orderBys == null)
			{
				orderings._orderBys = new List<BaseTableQuery.Ordering>();
			}
			orderings._orderBys.Add(new BaseTableQuery.Ordering()
			{
				ColumnName = this.Table.FindColumnWithPropertyName(memberExpression.Member.Name).Name,
				Ascending = asc
			});
			return orderings;
		}

		private void AddWhere(Expression pred)
		{
			if (this._where == null)
			{
				this._where = pred;
				return;
			}
			this._where = Expression.AndAlso(this._where, pred);
		}

		public TableQuery<U> Clone<U>()
		{
			TableQuery<U> us = new TableQuery<U>(this.Connection, this.Table)
			{
				_where = this._where,
				_deferred = this._deferred
			};
			if (this._orderBys != null)
			{
				us._orderBys = new List<BaseTableQuery.Ordering>(this._orderBys);
			}
			us._limit = this._limit;
			us._offset = this._offset;
			us._joinInner = this._joinInner;
			us._joinInnerKeySelector = this._joinInnerKeySelector;
			us._joinOuter = this._joinOuter;
			us._joinOuterKeySelector = this._joinOuterKeySelector;
			us._joinSelector = this._joinSelector;
			us._selector = this._selector;
			return us;
		}

		private TableQuery<T>.CompileResult CompileExpr(Expression expr, List<object> queryArgs)
		{
			string str;
			TableQuery<T>.CompileResult compileResult;
			if (expr == null)
			{
				throw new NotSupportedException("Expression is NULL");
			}
			if (expr is BinaryExpression)
			{
				BinaryExpression binaryExpression = (BinaryExpression)expr;
				TableQuery<T>.CompileResult compileResult1 = this.CompileExpr(binaryExpression.Left, queryArgs);
				TableQuery<T>.CompileResult compileResult2 = this.CompileExpr(binaryExpression.Right, queryArgs);
				if (!(compileResult1.CommandText == "?") || compileResult1.Value != null)
				{
					str = (!(compileResult2.CommandText == "?") || compileResult2.Value != null ? string.Concat(new string[] { "(", compileResult1.CommandText, " ", this.GetSqlName(binaryExpression), " ", compileResult2.CommandText, ")" }) : this.CompileNullBinaryExpression(binaryExpression, compileResult1));
				}
				else
				{
					str = this.CompileNullBinaryExpression(binaryExpression, compileResult2);
				}
				return new TableQuery<T>.CompileResult()
				{
					CommandText = str
				};
			}
			if (expr.NodeType != ExpressionType.Call)
			{
				if (expr.NodeType == ExpressionType.Constant)
				{
					ConstantExpression constantExpression = (ConstantExpression)expr;
					queryArgs.Add(constantExpression.Value);
					return new TableQuery<T>.CompileResult()
					{
						CommandText = "?",
						Value = constantExpression.Value
					};
				}
				if (expr.NodeType == ExpressionType.Convert)
				{
					UnaryExpression unaryExpression = (UnaryExpression)expr;
					Type type = unaryExpression.Type;
					TableQuery<T>.CompileResult compileResult3 = this.CompileExpr(unaryExpression.Operand, queryArgs);
					return new TableQuery<T>.CompileResult()
					{
						CommandText = compileResult3.CommandText,
						Value = (compileResult3.Value != null ? TableQuery<T>.ConvertTo(compileResult3.Value, type) : null)
					};
				}
				if (expr.NodeType != ExpressionType.MemberAccess)
				{
					ExpressionType nodeType = expr.NodeType;
					throw new NotSupportedException(string.Concat("Cannot compile: ", nodeType.ToString()));
				}
				MemberExpression memberExpression = (MemberExpression)expr;
				if (memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
				{
					string name = this.Table.FindColumnWithPropertyName(memberExpression.Member.Name).Name;
					return new TableQuery<T>.CompileResult()
					{
						CommandText = string.Concat("\"", name, "\"")
					};
				}
				object value = null;
				if (memberExpression.Expression != null)
				{
					TableQuery<T>.CompileResult compileResult4 = this.CompileExpr(memberExpression.Expression, queryArgs);
					if (compileResult4.Value == null)
					{
						throw new NotSupportedException("Member access failed to compile expression");
					}
					if (compileResult4.CommandText == "?")
					{
						queryArgs.RemoveAt(queryArgs.Count - 1);
					}
					value = compileResult4.Value;
				}
				object obj = null;
				if (memberExpression.Member.MemberType != MemberTypes.Property)
				{
					if (memberExpression.Member.MemberType != MemberTypes.Field)
					{
						throw new NotSupportedException(string.Concat("MemberExpr: ", memberExpression.Member.MemberType));
					}
					obj = ((FieldInfo)memberExpression.Member).GetValue(value);
				}
				else
				{
					obj = ((PropertyInfo)memberExpression.Member).GetValue(value, null);
				}
				if (obj == null || !(obj is IEnumerable) || obj is string || obj is IEnumerable<byte>)
				{
					queryArgs.Add(obj);
					return new TableQuery<T>.CompileResult()
					{
						CommandText = "?",
						Value = obj
					};
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("(");
				string str1 = "";
				foreach (object obj1 in (IEnumerable)obj)
				{
					queryArgs.Add(obj1);
					stringBuilder.Append(str1);
					stringBuilder.Append("?");
					str1 = ",";
				}
				stringBuilder.Append(")");
				return new TableQuery<T>.CompileResult()
				{
					CommandText = stringBuilder.ToString(),
					Value = obj
				};
			}
			MethodCallExpression methodCallExpression = (MethodCallExpression)expr;
			TableQuery<T>.CompileResult[] compileResultArray = new TableQuery<T>.CompileResult[methodCallExpression.Arguments.Count];
			if (methodCallExpression.Object != null)
			{
				compileResult = this.CompileExpr(methodCallExpression.Object, queryArgs);
			}
			else
			{
				compileResult = null;
			}
			TableQuery<T>.CompileResult compileResult5 = compileResult;
			for (int i = 0; i < (int)compileResultArray.Length; i++)
			{
				compileResultArray[i] = this.CompileExpr(methodCallExpression.Arguments[i], queryArgs);
			}
			string str2 = "";
			if (methodCallExpression.Method.Name == "Like" && (int)compileResultArray.Length == 2)
			{
				str2 = string.Concat(new string[] { "(", compileResultArray[0].CommandText, " like ", compileResultArray[1].CommandText, ")" });
			}
			else if (methodCallExpression.Method.Name == "Contains" && (int)compileResultArray.Length == 2)
			{
				str2 = string.Concat(new string[] { "(", compileResultArray[1].CommandText, " in ", compileResultArray[0].CommandText, ")" });
			}
			else if (methodCallExpression.Method.Name == "Contains" && (int)compileResultArray.Length == 1)
			{
				str2 = (methodCallExpression.Object == null || !(methodCallExpression.Object.Type == typeof(string)) ? string.Concat(new string[] { "(", compileResultArray[0].CommandText, " in ", compileResult5.CommandText, ")" }) : string.Concat(new string[] { "(", compileResult5.CommandText, " like ('%' || ", compileResultArray[0].CommandText, " || '%'))" }));
			}
			else if (methodCallExpression.Method.Name == "StartsWith" && (int)compileResultArray.Length == 1)
			{
				str2 = string.Concat(new string[] { "(", compileResult5.CommandText, " like (", compileResultArray[0].CommandText, " || '%'))" });
			}
			else if (methodCallExpression.Method.Name == "EndsWith" && (int)compileResultArray.Length == 1)
			{
				str2 = string.Concat(new string[] { "(", compileResult5.CommandText, " like ('%' || ", compileResultArray[0].CommandText, "))" });
			}
			else if (methodCallExpression.Method.Name == "Equals" && (int)compileResultArray.Length == 1)
			{
				str2 = string.Concat(new string[] { "(", compileResult5.CommandText, " = (", compileResultArray[0].CommandText, "))" });
			}
			else if (methodCallExpression.Method.Name != "ToLower")
			{
				str2 = (methodCallExpression.Method.Name != "ToUpper" ? string.Concat(methodCallExpression.Method.Name.ToLower(), "(", string.Join(",", (
					from a in (IEnumerable<TableQuery<T>.CompileResult>)compileResultArray
					select a.CommandText).ToArray<string>()), ")") : string.Concat("(upper(", compileResult5.CommandText, "))"));
			}
			else
			{
				str2 = string.Concat("(lower(", compileResult5.CommandText, "))");
			}
			return new TableQuery<T>.CompileResult()
			{
				CommandText = str2
			};
		}

		private string CompileNullBinaryExpression(BinaryExpression expression, TableQuery<T>.CompileResult parameter)
		{
			if (expression.NodeType == ExpressionType.Equal)
			{
				return string.Concat("(", parameter.CommandText, " is ?)");
			}
			if (expression.NodeType != ExpressionType.NotEqual)
			{
				ExpressionType nodeType = expression.NodeType;
				throw new NotSupportedException(string.Concat("Cannot compile Null-BinaryExpression with type ", nodeType.ToString()));
			}
			return string.Concat("(", parameter.CommandText, " is not ?)");
		}

		private static object ConvertTo(object obj, Type t)
		{
			Type underlyingType = Nullable.GetUnderlyingType(t);
			if (underlyingType == null)
			{
				return Convert.ChangeType(obj, t);
			}
			if (obj == null)
			{
				return null;
			}
			return Convert.ChangeType(obj, underlyingType);
		}

		public int Count()
		{
			return this.GenerateCommand("count(*)").ExecuteScalar<int>();
		}

		public int Count(Expression<Func<T, bool>> predExpr)
		{
			return this.Where(predExpr).Count();
		}

		public TableQuery<T> Deferred()
		{
			TableQuery<T> ts = this.Clone<T>();
			ts._deferred = true;
			return ts;
		}

		public T ElementAt(int index)
		{
			return this.Skip(index).Take(1).First();
		}

		public T First()
		{
			return this.Take(1).ToList<T>().First<T>();
		}

		public T FirstOrDefault()
		{
			return this.Take(1).ToList<T>().FirstOrDefault<T>();
		}

		private SQLiteCommand GenerateCommand(string selectionList)
		{
			if (this._joinInner != null && this._joinOuter != null)
			{
				throw new NotSupportedException("Joins are not supported.");
			}
			string str = string.Concat(new string[] { "select ", selectionList, " from \"", this.Table.TableName, "\"" });
			List<object> objs = new List<object>();
			if (this._where != null)
			{
				TableQuery<T>.CompileResult compileResult = this.CompileExpr(this._where, objs);
				str = string.Concat(str, " where ", compileResult.CommandText);
			}
			if (this._orderBys != null && this._orderBys.Count > 0)
			{
				string str1 = string.Join(", ", (
					from o in this._orderBys
					select string.Concat("\"", o.ColumnName, "\"", (o.Ascending ? "" : " desc"))).ToArray<string>());
				str = string.Concat(str, " order by ", str1);
			}
			if (this._limit.HasValue)
			{
				str = string.Concat(str, " limit ", this._limit.Value);
			}
			if (this._offset.HasValue)
			{
				if (!this._limit.HasValue)
				{
					str = string.Concat(str, " limit -1 ");
				}
				str = string.Concat(str, " offset ", this._offset.Value);
			}
			return this.Connection.CreateCommand(str, objs.ToArray());
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (this._deferred)
			{
				return this.GenerateCommand("*").ExecuteDeferredQuery<T>().GetEnumerator();
			}
			return this.GenerateCommand("*").ExecuteQuery<T>().GetEnumerator();
		}

		private string GetSqlName(Expression expr)
		{
			ExpressionType nodeType = expr.NodeType;
			if (nodeType == ExpressionType.GreaterThan)
			{
				return ">";
			}
			if (nodeType == ExpressionType.GreaterThanOrEqual)
			{
				return ">=";
			}
			if (nodeType == ExpressionType.LessThan)
			{
				return "<";
			}
			if (nodeType == ExpressionType.LessThanOrEqual)
			{
				return "<=";
			}
			if (nodeType == ExpressionType.And)
			{
				return "&";
			}
			if (nodeType == ExpressionType.AndAlso)
			{
				return "and";
			}
			if (nodeType == ExpressionType.Or)
			{
				return "|";
			}
			if (nodeType == ExpressionType.OrElse)
			{
				return "or";
			}
			if (nodeType == ExpressionType.Equal)
			{
				return "=";
			}
			if (nodeType != ExpressionType.NotEqual)
			{
				throw new NotSupportedException(string.Concat("Cannot get SQL for: ", nodeType));
			}
			return "!=";
		}

		public TableQuery<TResult> Join<TInner, TKey, TResult>(TableQuery<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
		{
			return new TableQuery<TResult>(this.Connection, this.Connection.GetMapping(typeof(TResult), CreateFlags.None))
			{
				_joinOuter = this,
				_joinOuterKeySelector = outerKeySelector,
				_joinInner = inner,
				_joinInnerKeySelector = innerKeySelector,
				_joinSelector = resultSelector
			};
		}

		public TableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr)
		{
			return this.AddOrderBy<U>(orderExpr, true);
		}

		public TableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr)
		{
			return this.AddOrderBy<U>(orderExpr, false);
		}

		public TableQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
		{
			TableQuery<TResult> tResults = this.Clone<TResult>();
			tResults._selector = selector;
			return tResults;
		}

		public TableQuery<T> Skip(int n)
		{
			TableQuery<T> nullable = this.Clone<T>();
			nullable._offset = new int?(n);
			return nullable;
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public TableQuery<T> Take(int n)
		{
			TableQuery<T> nullable = this.Clone<T>();
			nullable._limit = new int?(n);
			return nullable;
		}

		public TableQuery<T> ThenBy<U>(Expression<Func<T, U>> orderExpr)
		{
			return this.AddOrderBy<U>(orderExpr, true);
		}

		public TableQuery<T> ThenByDescending<U>(Expression<Func<T, U>> orderExpr)
		{
			return this.AddOrderBy<U>(orderExpr, false);
		}

		public TableQuery<T> Where(Expression<Func<T, bool>> predExpr)
		{
			if (predExpr.NodeType != ExpressionType.Lambda)
			{
				throw new NotSupportedException("Must be a predicate");
			}
			Expression body = predExpr.Body;
			TableQuery<T> ts = this.Clone<T>();
			ts.AddWhere(body);
			return ts;
		}

		private class CompileResult
		{
			public string CommandText
			{
				get;
				set;
			}

			public object Value
			{
				get;
				set;
			}

			public CompileResult()
			{
			}
		}
	}
}