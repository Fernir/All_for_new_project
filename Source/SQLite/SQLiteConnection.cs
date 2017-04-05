using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace SQLite
{
	public class SQLiteConnection : IDisposable
	{
		private bool _open;

		private TimeSpan _busyTimeout;

		private Dictionary<string, TableMapping> _mappings;

		private Dictionary<string, TableMapping> _tables;

		private Stopwatch _sw;

		private long _elapsedMilliseconds;

		private int _transactionDepth;

		private Random _rand = new Random();

		internal readonly static IntPtr NullHandle;

		private static bool _preserveDuringLinkMagic;

		public TimeSpan BusyTimeout
		{
			get
			{
				return this._busyTimeout;
			}
			set
			{
				this._busyTimeout = value;
				if (this.Handle != SQLiteConnection.NullHandle)
				{
					SQLite3.BusyTimeout(this.Handle, (int)this._busyTimeout.TotalMilliseconds);
				}
			}
		}

		public string DatabasePath
		{
			get;
			private set;
		}

		public IntPtr Handle
		{
			get;
			private set;
		}

		public bool IsInTransaction
		{
			get
			{
				return this._transactionDepth > 0;
			}
		}

		public bool StoreDateTimeAsTicks
		{
			get;
			private set;
		}

		public IEnumerable<TableMapping> TableMappings
		{
			get
			{
				if (this._tables == null)
				{
					return Enumerable.Empty<TableMapping>();
				}
				return this._tables.Values;
			}
		}

		public bool TimeExecution
		{
			get;
			set;
		}

		public bool Trace
		{
			get;
			set;
		}

		static SQLiteConnection()
		{
			if (SQLiteConnection._preserveDuringLinkMagic)
			{
				(new SQLiteConnection.ColumnInfo()).Name = "magic";
			}
		}

		public SQLiteConnection(string databasePath, bool storeDateTimeAsTicks = false) : this(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks)
		{
		}

		public SQLiteConnection(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = false)
		{
			IntPtr intPtr;
			if (string.IsNullOrEmpty(databasePath))
			{
				throw new ArgumentException("Must be specified", "databasePath");
			}
			this.DatabasePath = databasePath;
			SQLite3.Result result = SQLite3.Open(SQLiteConnection.GetNullTerminatedUtf8(this.DatabasePath), out intPtr, (int)openFlags, IntPtr.Zero);
			this.Handle = intPtr;
			if (result != SQLite3.Result.OK)
			{
				throw SQLiteException.New(result, string.Format("Could not open database file: {0} ({1})", this.DatabasePath, result));
			}
			this._open = true;
			this.StoreDateTimeAsTicks = storeDateTimeAsTicks;
			this.BusyTimeout = TimeSpan.FromSeconds(0.1);
		}

		public void BeginTransaction()
		{
			if (Interlocked.CompareExchange(ref this._transactionDepth, 1, 0) != 0)
			{
				throw new InvalidOperationException("Cannot begin a transaction while already in a transaction.");
			}
			try
			{
				this.Execute("begin transaction", new object[0]);
			}
			catch (Exception exception)
			{
				SQLiteException sQLiteException = exception as SQLiteException;
				if (sQLiteException == null)
				{
					Interlocked.Decrement(ref this._transactionDepth);
				}
				else
				{
					switch (sQLiteException.Result)
					{
						case SQLite3.Result.Busy:
						case SQLite3.Result.NoMem:
						case SQLite3.Result.Interrupt:
						case SQLite3.Result.IOError:
						case SQLite3.Result.Full:
						{
							this.RollbackTo(null, true);
							break;
						}
					}
				}
				throw;
			}
		}

		public void Close()
		{
			if (this._open && this.Handle != SQLiteConnection.NullHandle)
			{
				try
				{
					if (this._mappings != null)
					{
						foreach (TableMapping value in this._mappings.Values)
						{
							value.Dispose();
						}
					}
					SQLite3.Result result = SQLite3.Close(this.Handle);
					if (result != SQLite3.Result.OK)
					{
						string errmsg = SQLite3.GetErrmsg(this.Handle);
						throw SQLiteException.New(result, errmsg);
					}
				}
				finally
				{
					this.Handle = SQLiteConnection.NullHandle;
					this._open = false;
				}
			}
		}

		public void Commit()
		{
			if (Interlocked.Exchange(ref this._transactionDepth, 0) != 0)
			{
				this.Execute("commit", new object[0]);
			}
		}

		public SQLiteCommand CreateCommand(string cmdText, params object[] ps)
		{
			if (!this._open)
			{
				throw SQLiteException.New(SQLite3.Result.Error, "Cannot create commands from unopened database");
			}
			SQLiteCommand sQLiteCommand = this.NewCommand();
			sQLiteCommand.CommandText = cmdText;
			object[] objArray = ps;
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				sQLiteCommand.Bind(objArray[i]);
			}
			return sQLiteCommand;
		}

		public int CreateIndex(string indexName, string tableName, string[] columnNames, bool unique = false)
		{
			object[] objArray = new object[] { tableName, string.Join("\", \"", columnNames), null, null };
			objArray[2] = (unique ? "unique" : "");
			objArray[3] = indexName;
			string str = string.Format("create {2} index if not exists \"{3}\" on \"{0}\"(\"{1}\")", objArray);
			return this.Execute(str, new object[0]);
		}

		public int CreateIndex(string indexName, string tableName, string columnName, bool unique = false)
		{
			return this.CreateIndex(indexName, tableName, new string[] { columnName }, unique);
		}

		public int CreateIndex(string tableName, string columnName, bool unique = false)
		{
			return this.CreateIndex(string.Concat(tableName, "_", columnName), tableName, columnName, unique);
		}

		public int CreateIndex(string tableName, string[] columnNames, bool unique = false)
		{
			return this.CreateIndex(string.Concat(tableName, "_", string.Join("_", columnNames)), tableName, columnNames, unique);
		}

		public void CreateIndex<T>(Expression<Func<T, object>> property, bool unique = false)
		{
			MemberExpression memberExpression;
			memberExpression = (property.Body.NodeType != ExpressionType.Convert ? property.Body as MemberExpression : ((UnaryExpression)property.Body).Operand as MemberExpression);
			PropertyInfo member = memberExpression.Member as PropertyInfo;
			if (member == null)
			{
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
			}
			string name = member.Name;
			TableMapping mapping = this.GetMapping<T>();
			string str = mapping.FindColumnWithPropertyName(name).Name;
			this.CreateIndex(mapping.TableName, str, unique);
		}

		public int CreateTable<T>(CreateFlags createFlags = 0)
		{
			return this.CreateTable(typeof(T), createFlags);
		}

		public int CreateTable(Type ty, CreateFlags createFlags = 0)
		{
			TableMapping mapping;
			SQLiteConnection.IndexInfo indexInfo;
			if (this._tables == null)
			{
				this._tables = new Dictionary<string, TableMapping>();
			}
			if (!this._tables.TryGetValue(ty.FullName, out mapping))
			{
				mapping = this.GetMapping(ty, createFlags);
				this._tables.Add(ty.FullName, mapping);
			}
			string str = string.Concat("create table if not exists \"", mapping.TableName, "\"(\n");
			IEnumerable<string> columns = 
				from p in mapping.Columns
				select Orm.SqlDecl(p, this.StoreDateTimeAsTicks);
			string str1 = string.Join(",\n", columns.ToArray<string>());
			str = string.Concat(str, str1);
			str = string.Concat(str, ")");
			int num = this.Execute(str, new object[0]);
			if (num == 0)
			{
				this.MigrateTable(mapping);
			}
			Dictionary<string, SQLiteConnection.IndexInfo> strs = new Dictionary<string, SQLiteConnection.IndexInfo>();
			TableMapping.Column[] columnArray = mapping.Columns;
			for (int i1 = 0; i1 < (int)columnArray.Length; i1++)
			{
				TableMapping.Column column = columnArray[i1];
				foreach (IndexedAttribute index in column.Indices)
				{
					string name = index.Name ?? string.Concat(mapping.TableName, "_", column.Name);
					if (!strs.TryGetValue(name, out indexInfo))
					{
						SQLiteConnection.IndexInfo indexInfo1 = new SQLiteConnection.IndexInfo()
						{
							IndexName = name,
							TableName = mapping.TableName,
							Unique = index.Unique,
							Columns = new List<SQLiteConnection.IndexedColumn>()
						};
						indexInfo = indexInfo1;
						strs.Add(name, indexInfo);
					}
					if (index.Unique != indexInfo.Unique)
					{
						throw new Exception("All the columns in an index must have the same value for their Unique property");
					}
					List<SQLiteConnection.IndexedColumn> indexedColumns = indexInfo.Columns;
					SQLiteConnection.IndexedColumn indexedColumn = new SQLiteConnection.IndexedColumn()
					{
						Order = index.Order,
						ColumnName = column.Name
					};
					indexedColumns.Add(indexedColumn);
				}
			}
			foreach (string key in strs.Keys)
			{
				SQLiteConnection.IndexInfo item = strs[key];
				string[] array = (
					from i in item.Columns
					orderby i.Order
					select i.ColumnName).ToArray<string>();
				num = num + this.CreateIndex(key, item.TableName, array, item.Unique);
			}
			return num;
		}

		public IEnumerable<T> DeferredQuery<T>(string query, params object[] args)
		where T : new()
		{
			return this.CreateCommand(query, args).ExecuteDeferredQuery<T>();
		}

		public IEnumerable<object> DeferredQuery(TableMapping map, string query, params object[] args)
		{
			return this.CreateCommand(query, args).ExecuteDeferredQuery<object>(map);
		}

		public int Delete(object objectToDelete)
		{
			TableMapping mapping = this.GetMapping(objectToDelete.GetType(), CreateFlags.None);
			TableMapping.Column pK = mapping.PK;
			if (pK == null)
			{
				throw new NotSupportedException(string.Concat("Cannot delete ", mapping.TableName, ": it has no PK"));
			}
			string str = string.Format("delete from \"{0}\" where \"{1}\" = ?", mapping.TableName, pK.Name);
			return this.Execute(str, new object[] { pK.GetValue(objectToDelete) });
		}

		public int Delete<T>(object primaryKey)
		{
			TableMapping mapping = this.GetMapping(typeof(T), CreateFlags.None);
			TableMapping.Column pK = mapping.PK;
			if (pK == null)
			{
				throw new NotSupportedException(string.Concat("Cannot delete ", mapping.TableName, ": it has no PK"));
			}
			string str = string.Format("delete from \"{0}\" where \"{1}\" = ?", mapping.TableName, pK.Name);
			return this.Execute(str, new object[] { primaryKey });
		}

		public int DeleteAll<T>()
		{
			TableMapping mapping = this.GetMapping(typeof(T), CreateFlags.None);
			string str = string.Format("delete from \"{0}\"", mapping.TableName);
			return this.Execute(str, new object[0]);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			this.Close();
		}

		private void DoSavePointExecute(string savepoint, string cmd)
		{
			int num;
			int num1 = savepoint.IndexOf('D');
			if (num1 < 2 || savepoint.Length <= num1 + 1 || !int.TryParse(savepoint.Substring(num1 + 1), out num) || 0 > num || num >= this._transactionDepth)
			{
				throw new ArgumentException("savePoint is not valid, and should be the result of a call to SaveTransactionPoint.", "savePoint");
			}
			Thread.VolatileWrite(ref this._transactionDepth, num);
			this.Execute(string.Concat(cmd, savepoint), new object[0]);
		}

		public int DropTable<T>()
		{
			TableMapping mapping = this.GetMapping(typeof(T), CreateFlags.None);
			string str = string.Format("drop table if exists \"{0}\"", mapping.TableName);
			return this.Execute(str, new object[0]);
		}

		public void EnableLoadExtension(int onoff)
		{
			SQLite3.Result result = SQLite3.EnableLoadExtension(this.Handle, onoff);
			if (result != SQLite3.Result.OK)
			{
				string errmsg = SQLite3.GetErrmsg(this.Handle);
				throw SQLiteException.New(result, errmsg);
			}
		}

		public int Execute(string query, params object[] args)
		{
			SQLiteCommand sQLiteCommand = this.CreateCommand(query, args);
			if (this.TimeExecution)
			{
				if (this._sw == null)
				{
					this._sw = new Stopwatch();
				}
				this._sw.Reset();
				this._sw.Start();
			}
			int num = sQLiteCommand.ExecuteNonQuery();
			if (this.TimeExecution)
			{
				this._sw.Stop();
				this._elapsedMilliseconds = this._elapsedMilliseconds + this._sw.ElapsedMilliseconds;
			}
			return num;
		}

		public T ExecuteScalar<T>(string query, params object[] args)
		{
			SQLiteCommand sQLiteCommand = this.CreateCommand(query, args);
			if (this.TimeExecution)
			{
				if (this._sw == null)
				{
					this._sw = new Stopwatch();
				}
				this._sw.Reset();
				this._sw.Start();
			}
			T t = sQLiteCommand.ExecuteScalar<T>();
			if (this.TimeExecution)
			{
				this._sw.Stop();
				this._elapsedMilliseconds = this._elapsedMilliseconds + this._sw.ElapsedMilliseconds;
			}
			return t;
		}

		~SQLiteConnection()
		{
			this.Dispose(false);
		}

		public T Find<T>(object pk)
		where T : new()
		{
			TableMapping mapping = this.GetMapping(typeof(T), CreateFlags.None);
			return this.Query<T>(mapping.GetByPrimaryKeySql, new object[] { pk }).FirstOrDefault<T>();
		}

		public object Find(object pk, TableMapping map)
		{
			return this.Query(map, map.GetByPrimaryKeySql, new object[] { pk }).FirstOrDefault<object>();
		}

		public T Find<T>(Expression<Func<T, bool>> predicate)
		where T : new()
		{
			return this.Table<T>().Where(predicate).FirstOrDefault();
		}

		public T Get<T>(object pk)
		where T : new()
		{
			TableMapping mapping = this.GetMapping(typeof(T), CreateFlags.None);
			return this.Query<T>(mapping.GetByPrimaryKeySql, new object[] { pk }).First<T>();
		}

		public T Get<T>(Expression<Func<T, bool>> predicate)
		where T : new()
		{
			return this.Table<T>().Where(predicate).First();
		}

		public TableMapping GetMapping(Type type, CreateFlags createFlags = 0)
		{
			TableMapping tableMapping;
			if (this._mappings == null)
			{
				this._mappings = new Dictionary<string, TableMapping>();
			}
			if (!this._mappings.TryGetValue(type.FullName, out tableMapping))
			{
				tableMapping = new TableMapping(type, createFlags);
				this._mappings[type.FullName] = tableMapping;
			}
			return tableMapping;
		}

		public TableMapping GetMapping<T>()
		{
			return this.GetMapping(typeof(T), CreateFlags.None);
		}

		private static byte[] GetNullTerminatedUtf8(string s)
		{
			byte[] numArray = new byte[Encoding.UTF8.GetByteCount(s) + 1];
			Encoding.UTF8.GetBytes(s, 0, s.Length, numArray, 0);
			return numArray;
		}

		public List<SQLiteConnection.ColumnInfo> GetTableInfo(string tableName)
		{
			string str = string.Concat("pragma table_info(\"", tableName, "\")");
			return this.Query<SQLiteConnection.ColumnInfo>(str, new object[0]);
		}

		public int Insert(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return this.Insert(obj, "", obj.GetType());
		}

		public int Insert(object obj, Type objType)
		{
			return this.Insert(obj, "", objType);
		}

		public int Insert(object obj, string extra)
		{
			if (obj == null)
			{
				return 0;
			}
			return this.Insert(obj, extra, obj.GetType());
		}

		public int Insert(object obj, string extra, Type objType)
		{
			int num;
			if (obj == null || objType == null)
			{
				return 0;
			}
			TableMapping mapping = this.GetMapping(objType, CreateFlags.None);
			if (mapping.PK != null && mapping.PK.IsAutoGuid)
			{
				PropertyInfo property = objType.GetProperty(mapping.PK.PropertyName);
				if (property != null && property.GetValue(obj, null).Equals(Guid.Empty))
				{
					property.SetValue(obj, Guid.NewGuid(), null);
				}
			}
			TableMapping.Column[] columnArray = (string.Compare(extra, "OR REPLACE", StringComparison.OrdinalIgnoreCase) == 0 ? mapping.InsertOrReplaceColumns : mapping.InsertColumns);
			object[] value = new object[(int)columnArray.Length];
			for (int i = 0; i < (int)value.Length; i++)
			{
				value[i] = columnArray[i].GetValue(obj);
			}
			PreparedSqlLiteInsertCommand insertCommand = mapping.GetInsertCommand(this, extra);
			try
			{
				num = insertCommand.ExecuteNonQuery(value);
			}
			catch (SQLiteException sQLiteException1)
			{
				SQLiteException sQLiteException = sQLiteException1;
				if (SQLite3.ExtendedErrCode(this.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
				{
					throw NotNullConstraintViolationException.New(sQLiteException.Result, sQLiteException.Message, mapping, obj);
				}
				throw;
			}
			if (mapping.HasAutoIncPK)
			{
				mapping.SetAutoIncPK(obj, SQLite3.LastInsertRowid(this.Handle));
			}
			return num;
		}

		public int InsertAll(IEnumerable objects)
		{
			int num = 0;
			this.RunInTransaction(() => {
				foreach (object @object in objects)
				{
					num = num + this.Insert(@object);
				}
			});
			return num;
		}

		public int InsertAll(IEnumerable objects, string extra)
		{
			int num = 0;
			this.RunInTransaction(() => {
				foreach (object @object in objects)
				{
					num = num + this.Insert(@object, extra);
				}
			});
			return num;
		}

		public int InsertAll(IEnumerable objects, Type objType)
		{
			int num = 0;
			this.RunInTransaction(() => {
				foreach (object @object in objects)
				{
					num = num + this.Insert(@object, objType);
				}
			});
			return num;
		}

		public int InsertOrReplace(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return this.Insert(obj, "OR REPLACE", obj.GetType());
		}

		public int InsertOrReplace(object obj, Type objType)
		{
			return this.Insert(obj, "OR REPLACE", objType);
		}

		private void MigrateTable(TableMapping map)
		{
			List<SQLiteConnection.ColumnInfo> tableInfo = this.GetTableInfo(map.TableName);
			List<TableMapping.Column> columns = new List<TableMapping.Column>();
			TableMapping.Column[] columnArray = map.Columns;
			for (int i = 0; i < (int)columnArray.Length; i++)
			{
				TableMapping.Column column = columnArray[i];
				bool flag = false;
				foreach (SQLiteConnection.ColumnInfo columnInfo in tableInfo)
				{
					flag = string.Compare(column.Name, columnInfo.Name, StringComparison.OrdinalIgnoreCase) == 0;
					if (!flag)
					{
						continue;
					}
					goto Label0;
				}
			Label0:
				if (!flag)
				{
					columns.Add(column);
				}
			}
			foreach (TableMapping.Column column1 in columns)
			{
				string str = string.Concat("alter table \"", map.TableName, "\" add column ", Orm.SqlDecl(column1, this.StoreDateTimeAsTicks));
				this.Execute(str, new object[0]);
			}
		}

		protected virtual SQLiteCommand NewCommand()
		{
			return new SQLiteCommand(this);
		}

		public List<T> Query<T>(string query, params object[] args)
		where T : new()
		{
			return this.CreateCommand(query, args).ExecuteQuery<T>();
		}

		public List<object> Query(TableMapping map, string query, params object[] args)
		{
			return this.CreateCommand(query, args).ExecuteQuery<object>(map);
		}

		public void Release(string savepoint)
		{
			this.DoSavePointExecute(savepoint, "release ");
		}

		public void Rollback()
		{
			this.RollbackTo(null, false);
		}

		public void RollbackTo(string savepoint)
		{
			this.RollbackTo(savepoint, false);
		}

		private void RollbackTo(string savepoint, bool noThrow)
		{
			try
			{
				if (!string.IsNullOrEmpty(savepoint))
				{
					this.DoSavePointExecute(savepoint, "rollback to ");
				}
				else if (Interlocked.Exchange(ref this._transactionDepth, 0) > 0)
				{
					this.Execute("rollback", new object[0]);
				}
			}
			catch (SQLiteException sQLiteException)
			{
				if (!noThrow)
				{
					throw;
				}
			}
		}

		public void RunInTransaction(Action action)
		{
			try
			{
				string str = this.SaveTransactionPoint();
				action();
				this.Release(str);
			}
			catch (Exception exception)
			{
				this.Rollback();
				throw;
			}
		}

		public string SaveTransactionPoint()
		{
			int num = Interlocked.Increment(ref this._transactionDepth) - 1;
			string str = string.Concat(new object[] { "S", this._rand.Next(32767), "D", num });
			try
			{
				this.Execute(string.Concat("savepoint ", str), new object[0]);
			}
			catch (Exception exception)
			{
				SQLiteException sQLiteException = exception as SQLiteException;
				if (sQLiteException == null)
				{
					Interlocked.Decrement(ref this._transactionDepth);
				}
				else
				{
					switch (sQLiteException.Result)
					{
						case SQLite3.Result.Busy:
						case SQLite3.Result.NoMem:
						case SQLite3.Result.Interrupt:
						case SQLite3.Result.IOError:
						case SQLite3.Result.Full:
						{
							this.RollbackTo(null, true);
							break;
						}
					}
				}
				throw;
			}
			return str;
		}

		public TableQuery<T> Table<T>()
		where T : new()
		{
			return new TableQuery<T>(this);
		}

		public int Update(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return this.Update(obj, obj.GetType());
		}

		public int Update(object obj, Type objType)
		{
			int num = 0;
			if (obj == null || objType == null)
			{
				return 0;
			}
			TableMapping mapping = this.GetMapping(objType, CreateFlags.None);
			TableMapping.Column pK = mapping.PK;
			if (pK == null)
			{
				throw new NotSupportedException(string.Concat("Cannot update ", mapping.TableName, ": it has no PK"));
			}
			IEnumerable<TableMapping.Column> columns = 
				from p in mapping.Columns
				where p != pK
				select p;
			List<object> objs = new List<object>(
				from c in columns
				select c.GetValue(obj))
			{
				pK.GetValue(obj)
			};
			string str = string.Format("update \"{0}\" set {1} where {2} = ? ", mapping.TableName, string.Join(",", (
				from c in columns
				select string.Concat("\"", c.Name, "\" = ? ")).ToArray<string>()), pK.Name);
			try
			{
				num = this.Execute(str, objs.ToArray());
			}
			catch (SQLiteException sQLiteException1)
			{
				SQLiteException sQLiteException = sQLiteException1;
				if (sQLiteException.Result != SQLite3.Result.Constraint || SQLite3.ExtendedErrCode(this.Handle) != SQLite3.ExtendedResult.ConstraintNotNull)
				{
					throw sQLiteException;
				}
				throw NotNullConstraintViolationException.New(sQLiteException, mapping, obj);
			}
			return num;
		}

		public int UpdateAll(IEnumerable objects)
		{
			int num = 0;
			this.RunInTransaction(() => {
				foreach (object @object in objects)
				{
					num = num + this.Update(@object);
				}
			});
			return num;
		}

		public class ColumnInfo
		{
			[Column("name")]
			public string Name
			{
				get;
				set;
			}

			public int notnull
			{
				get;
				set;
			}

			public ColumnInfo()
			{
			}

			public override string ToString()
			{
				return this.Name;
			}
		}

		private struct IndexedColumn
		{
			public int Order;

			public string ColumnName;
		}

		private struct IndexInfo
		{
			public string IndexName;

			public string TableName;

			public bool Unique;

			public List<SQLiteConnection.IndexedColumn> Columns;
		}
	}
}