using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	public class SQLiteException : Exception
	{
		public SQLite3.Result Result
		{
			get;
			private set;
		}

		protected SQLiteException(SQLite3.Result r, string message) : base(message)
		{
			this.Result = r;
		}

		public static SQLiteException New(SQLite3.Result r, string message)
		{
			return new SQLiteException(r, message);
		}
	}
}