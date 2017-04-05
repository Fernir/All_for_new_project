using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	internal class SQLiteConnectionString
	{
		public string ConnectionString
		{
			get;
			private set;
		}

		public string DatabasePath
		{
			get;
			private set;
		}

		public bool StoreDateTimeAsTicks
		{
			get;
			private set;
		}

		public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks)
		{
			this.ConnectionString = databasePath;
			this.StoreDateTimeAsTicks = storeDateTimeAsTicks;
			this.DatabasePath = databasePath;
		}
	}
}