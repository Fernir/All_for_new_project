using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	public class PreparedSqlLiteInsertCommand : IDisposable
	{
		internal readonly static IntPtr NullStatement;

		public string CommandText
		{
			get;
			set;
		}

		protected SQLiteConnection Connection
		{
			get;
			set;
		}

		public bool Initialized
		{
			get;
			set;
		}

		protected IntPtr Statement
		{
			get;
			set;
		}

		static PreparedSqlLiteInsertCommand()
		{
		}

		internal PreparedSqlLiteInsertCommand(SQLiteConnection conn)
		{
			this.Connection = conn;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (this.Statement != PreparedSqlLiteInsertCommand.NullStatement)
			{
				try
				{
					SQLite3.Finalize(this.Statement);
				}
				finally
				{
					this.Statement = PreparedSqlLiteInsertCommand.NullStatement;
					this.Connection = null;
				}
			}
		}

		public int ExecuteNonQuery(object[] source)
		{
			bool trace = this.Connection.Trace;
			SQLite3.Result result = SQLite3.Result.OK;
			if (!this.Initialized)
			{
				this.Statement = this.Prepare();
				this.Initialized = true;
			}
			if (source != null)
			{
				for (int i = 0; i < (int)source.Length; i++)
				{
					SQLiteCommand.BindParameter(this.Statement, i + 1, source[i], this.Connection.StoreDateTimeAsTicks);
				}
			}
			result = SQLite3.Step(this.Statement);
			if (result != SQLite3.Result.Done)
			{
				if (result != SQLite3.Result.Error)
				{
					if (result != SQLite3.Result.Constraint || SQLite3.ExtendedErrCode(this.Connection.Handle) != SQLite3.ExtendedResult.ConstraintNotNull)
					{
						SQLite3.Reset(this.Statement);
						throw SQLiteException.New(result, result.ToString());
					}
					SQLite3.Reset(this.Statement);
					throw NotNullConstraintViolationException.New(result, SQLite3.GetErrmsg(this.Connection.Handle));
				}
				string errmsg = SQLite3.GetErrmsg(this.Connection.Handle);
				SQLite3.Reset(this.Statement);
				throw SQLiteException.New(result, errmsg);
			}
			int num = SQLite3.Changes(this.Connection.Handle);
			SQLite3.Reset(this.Statement);
			return num;
		}

		~PreparedSqlLiteInsertCommand()
		{
			this.Dispose(false);
		}

		protected virtual IntPtr Prepare()
		{
			return SQLite3.Prepare2(this.Connection.Handle, this.CommandText);
		}
	}
}