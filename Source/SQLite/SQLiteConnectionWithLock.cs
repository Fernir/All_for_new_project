using System;
using System.Threading;

namespace SQLite
{
	internal class SQLiteConnectionWithLock : SQLiteConnection
	{
		private readonly object _lockPoint = new object();

		public SQLiteConnectionWithLock(SQLiteConnectionString connectionString, SQLiteOpenFlags openFlags) : base(connectionString.DatabasePath, openFlags, connectionString.StoreDateTimeAsTicks)
		{
		}

		public IDisposable Lock()
		{
			return new SQLiteConnectionWithLock.LockWrapper(this._lockPoint);
		}

		private class LockWrapper : IDisposable
		{
			private object _lockPoint;

			public LockWrapper(object lockPoint)
			{
				this._lockPoint = lockPoint;
				Monitor.Enter(this._lockPoint);
			}

			public void Dispose()
			{
				Monitor.Exit(this._lockPoint);
			}
		}
	}
}