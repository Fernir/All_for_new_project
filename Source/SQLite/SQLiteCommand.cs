using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SQLite
{
	public class SQLiteCommand
	{
		private SQLiteConnection _conn;

		private List<SQLiteCommand.Binding> _bindings;

		internal static IntPtr NegativePointer;

		public string CommandText
		{
			get;
			set;
		}

		static SQLiteCommand()
		{
			SQLiteCommand.NegativePointer = new IntPtr(-1);
		}

		internal SQLiteCommand(SQLiteConnection conn)
		{
			this._conn = conn;
			this._bindings = new List<SQLiteCommand.Binding>();
			this.CommandText = "";
		}

		public void Bind(string name, object val)
		{
			this._bindings.Add(new SQLiteCommand.Binding()
			{
				Name = name,
				Value = val
			});
		}

		public void Bind(object val)
		{
			this.Bind(null, val);
		}

		private void BindAll(IntPtr stmt)
		{
			int num = 1;
			foreach (SQLiteCommand.Binding _binding in this._bindings)
			{
				if (_binding.Name == null)
				{
					int num1 = num;
					num = num1 + 1;
					_binding.Index = num1;
				}
				else
				{
					_binding.Index = SQLite3.BindParameterIndex(stmt, _binding.Name);
				}
				SQLiteCommand.BindParameter(stmt, _binding.Index, _binding.Value, this._conn.StoreDateTimeAsTicks);
			}
		}

		internal static void BindParameter(IntPtr stmt, int index, object value, bool storeDateTimeAsTicks)
		{
			DateTime dateTime;
			if (value == null)
			{
				SQLite3.BindNull(stmt, index);
				return;
			}
			if (value is int)
			{
				SQLite3.BindInt(stmt, index, (int)value);
				return;
			}
			if (value is string)
			{
				SQLite3.BindText(stmt, index, (string)value, -1, SQLiteCommand.NegativePointer);
				return;
			}
			if (value is byte || value is ushort || value is sbyte || value is short)
			{
				SQLite3.BindInt(stmt, index, Convert.ToInt32(value));
				return;
			}
			if (value is bool)
			{
				SQLite3.BindInt(stmt, index, ((bool)value ? 1 : 0));
				return;
			}
			if (value is uint || value is long)
			{
				SQLite3.BindInt64(stmt, index, Convert.ToInt64(value));
				return;
			}
			if (value is float || value is double || value is decimal)
			{
				SQLite3.BindDouble(stmt, index, Convert.ToDouble(value));
				return;
			}
			if (value is TimeSpan)
			{
				TimeSpan timeSpan = (TimeSpan)value;
				SQLite3.BindInt64(stmt, index, timeSpan.Ticks);
				return;
			}
			if (value is DateTime)
			{
				if (storeDateTimeAsTicks)
				{
					dateTime = (DateTime)value;
					SQLite3.BindInt64(stmt, index, dateTime.Ticks);
					return;
				}
				dateTime = (DateTime)value;
				SQLite3.BindText(stmt, index, dateTime.ToString("yyyy-MM-dd HH:mm:ss"), -1, SQLiteCommand.NegativePointer);
				return;
			}
			if (value is DateTimeOffset)
			{
				DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
				SQLite3.BindInt64(stmt, index, dateTimeOffset.UtcTicks);
				return;
			}
			if (value.GetType().IsEnum)
			{
				SQLite3.BindInt(stmt, index, Convert.ToInt32(value));
				return;
			}
			if (value is byte[])
			{
				SQLite3.BindBlob(stmt, index, (byte[])value, (int)((byte[])value).Length, SQLiteCommand.NegativePointer);
				return;
			}
			if (!(value is Guid))
			{
				throw new NotSupportedException(string.Concat("Cannot store type: ", value.GetType()));
			}
			Guid guid = (Guid)value;
			SQLite3.BindText(stmt, index, guid.ToString(), 72, SQLiteCommand.NegativePointer);
		}

		public IEnumerable<T> ExecuteDeferredQuery<T>()
		{
			return this.ExecuteDeferredQuery<T>(this._conn.GetMapping(typeof(T), CreateFlags.None));
		}

		public IEnumerable<T> ExecuteDeferredQuery<T>(TableMapping map)
		{
			bool trace = this._conn.Trace;
			IntPtr intPtr = this.Prepare();
			try
			{
				TableMapping.Column[] columnArray = new TableMapping.Column[SQLite3.ColumnCount(intPtr)];
				for (int i = 0; i < (int)columnArray.Length; i++)
				{
					string str = SQLite3.ColumnName16(intPtr, i);
					columnArray[i] = map.FindColumn(str);
				}
				while (SQLite3.Step(intPtr) == SQLite3.Result.Row)
				{
					object obj = Activator.CreateInstance(map.MappedType);
					for (int j = 0; j < (int)columnArray.Length; j++)
					{
						if (columnArray[j] != null)
						{
							SQLite3.ColType colType = SQLite3.ColumnType(intPtr, j);
							object obj1 = this.ReadCol(intPtr, j, colType, columnArray[j].ColumnType);
							columnArray[j].SetValue(obj, obj1);
						}
					}
					this.OnInstanceCreated(obj);
					yield return (T)obj;
				}
				columnArray = null;
			}
			finally
			{
				SQLite3.Finalize(intPtr);
			}
		}

		public int ExecuteNonQuery()
		{
			bool trace = this._conn.Trace;
			SQLite3.Result result = SQLite3.Result.OK;
			IntPtr intPtr = this.Prepare();
			result = SQLite3.Step(intPtr);
			this.Finalize(intPtr);
			if (result != SQLite3.Result.Done)
			{
				if (result != SQLite3.Result.Error)
				{
					if (result != SQLite3.Result.Constraint || SQLite3.ExtendedErrCode(this._conn.Handle) != SQLite3.ExtendedResult.ConstraintNotNull)
					{
						throw SQLiteException.New(result, result.ToString());
					}
					throw NotNullConstraintViolationException.New(result, SQLite3.GetErrmsg(this._conn.Handle));
				}
				string errmsg = SQLite3.GetErrmsg(this._conn.Handle);
				throw SQLiteException.New(result, errmsg);
			}
			return SQLite3.Changes(this._conn.Handle);
		}

		public List<T> ExecuteQuery<T>()
		{
			return this.ExecuteDeferredQuery<T>(this._conn.GetMapping(typeof(T), CreateFlags.None)).ToList<T>();
		}

		public List<T> ExecuteQuery<T>(TableMapping map)
		{
			return this.ExecuteDeferredQuery<T>(map).ToList<T>();
		}

		public T ExecuteScalar<T>()
		{
			bool trace = this._conn.Trace;
			T t = default(T);
			IntPtr intPtr = this.Prepare();
			try
			{
				SQLite3.Result result = SQLite3.Step(intPtr);
				if (result == SQLite3.Result.Row)
				{
					SQLite3.ColType colType = SQLite3.ColumnType(intPtr, 0);
					t = (T)this.ReadCol(intPtr, 0, colType, typeof(T));
				}
				else if (result != SQLite3.Result.Done)
				{
					throw SQLiteException.New(result, SQLite3.GetErrmsg(this._conn.Handle));
				}
			}
			finally
			{
				this.Finalize(intPtr);
			}
			return t;
		}

		private void Finalize(IntPtr stmt)
		{
			SQLite3.Finalize(stmt);
		}

		protected virtual void OnInstanceCreated(object obj)
		{
		}

		private IntPtr Prepare()
		{
			IntPtr intPtr = SQLite3.Prepare2(this._conn.Handle, this.CommandText);
			this.BindAll(intPtr);
			return intPtr;
		}

		private object ReadCol(IntPtr stmt, int index, SQLite3.ColType type, Type clrType)
		{
			if (type == SQLite3.ColType.Null)
			{
				return null;
			}
			if (clrType == typeof(string))
			{
				return SQLite3.ColumnString(stmt, index);
			}
			if (clrType == typeof(int))
			{
				return SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(bool))
			{
				return SQLite3.ColumnInt(stmt, index) == 1;
			}
			if (clrType == typeof(double))
			{
				return SQLite3.ColumnDouble(stmt, index);
			}
			if (clrType == typeof(float))
			{
				return (float)SQLite3.ColumnDouble(stmt, index);
			}
			if (clrType == typeof(TimeSpan))
			{
				return new TimeSpan(SQLite3.ColumnInt64(stmt, index));
			}
			if (clrType == typeof(DateTime))
			{
				if (this._conn.StoreDateTimeAsTicks)
				{
					return new DateTime(SQLite3.ColumnInt64(stmt, index));
				}
				return DateTime.Parse(SQLite3.ColumnString(stmt, index));
			}
			if (clrType == typeof(DateTimeOffset))
			{
				return new DateTimeOffset(SQLite3.ColumnInt64(stmt, index), TimeSpan.Zero);
			}
			if (clrType.IsEnum)
			{
				return SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(long))
			{
				return SQLite3.ColumnInt64(stmt, index);
			}
			if (clrType == typeof(uint))
			{
				return (uint)SQLite3.ColumnInt64(stmt, index);
			}
			if (clrType == typeof(decimal))
			{
				return (decimal)SQLite3.ColumnDouble(stmt, index);
			}
			if (clrType == typeof(byte))
			{
				return (byte)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(ushort))
			{
				return (ushort)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(short))
			{
				return (short)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(sbyte))
			{
				return (sbyte)SQLite3.ColumnInt(stmt, index);
			}
			if (clrType == typeof(byte[]))
			{
				return SQLite3.ColumnByteArray(stmt, index);
			}
			if (clrType != typeof(Guid))
			{
				throw new NotSupportedException(string.Concat("Don't know how to read ", clrType));
			}
			return new Guid(SQLite3.ColumnString(stmt, index));
		}

		public override string ToString()
		{
			string[] commandText = new string[1 + this._bindings.Count];
			commandText[0] = this.CommandText;
			int num = 1;
			foreach (SQLiteCommand.Binding _binding in this._bindings)
			{
				commandText[num] = string.Format("  {0}: {1}", num - 1, _binding.Value);
				num++;
			}
			return string.Join(Environment.NewLine, commandText);
		}

		private class Binding
		{
			public int Index
			{
				get;
				set;
			}

			public string Name
			{
				get;
				set;
			}

			public object Value
			{
				get;
				set;
			}

			public Binding()
			{
			}
		}
	}
}