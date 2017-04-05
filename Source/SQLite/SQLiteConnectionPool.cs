using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SQLite
{
	internal class SQLiteConnectionPool
	{
		private readonly Dictionary<string, SQLiteConnectionPool.Entry> _entries = new Dictionary<string, SQLiteConnectionPool.Entry>();

		private readonly object _entriesLock = new object();

		private readonly static SQLiteConnectionPool _shared;

		public static SQLiteConnectionPool Shared
		{
			get
			{
				return SQLiteConnectionPool._shared;
			}
		}

		static SQLiteConnectionPool()
		{
			SQLiteConnectionPool._shared = new SQLiteConnectionPool();
		}

		public SQLiteConnectionPool()
		{
		}

		public void ApplicationSuspended()
		{
			this.Reset();
		}

		public SQLiteConnectionWithLock GetConnection(SQLiteConnectionString connectionString, SQLiteOpenFlags openFlags)
		{
			SQLiteConnectionPool.Entry entry;
			SQLiteConnectionWithLock connection;
			lock (this._entriesLock)
			{
				string str = connectionString.ConnectionString;
				if (!this._entries.TryGetValue(str, out entry))
				{
					entry = new SQLiteConnectionPool.Entry(connectionString, openFlags);
					this._entries[str] = entry;
				}
				connection = entry.Connection;
			}
			return connection;
		}

		public void Reset()
		{
			lock (this._entriesLock)
			{
				foreach (SQLiteConnectionPool.Entry value in this._entries.Values)
				{
					value.OnApplicationSuspended();
				}
				this._entries.Clear();
			}
		}

		private class Entry
		{
			public SQLiteConnectionWithLock Connection
			{
				get;
				private set;
			}

			public SQLiteConnectionString ConnectionString
			{
				get;
				private set;
			}

			public Entry(SQLiteConnectionString connectionString, SQLiteOpenFlags openFlags)
			{
				this.ConnectionString = connectionString;
				this.Connection = new SQLiteConnectionWithLock(connectionString, openFlags);
			}

			public void OnApplicationSuspended()
			{
				this.Connection.Dispose();
				this.Connection = null;
			}
		}
	}
}