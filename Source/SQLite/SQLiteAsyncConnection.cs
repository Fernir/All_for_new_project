using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SQLite
{
	public class SQLiteAsyncConnection
	{
		private SQLiteConnectionString _connectionString;

		private SQLiteOpenFlags _openFlags;

		public SQLiteAsyncConnection(string databasePath, bool storeDateTimeAsTicks = false) : this(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks)
		{
		}

		public SQLiteAsyncConnection(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = false)
		{
			this._openFlags = openFlags;
			this._connectionString = new SQLiteConnectionString(databasePath, storeDateTimeAsTicks);
		}

		public Task<CreateTablesResult> CreateTableAsync<T>()
		where T : new()
		{
			return this.CreateTablesAsync(new Type[] { typeof(T) });
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2>()
		where T : new()
		where T2 : new()
		{
			return this.CreateTablesAsync(new Type[] { typeof(T), typeof(T2) });
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3>()
		where T : new()
		where T2 : new()
		where T3 : new()
		{
			return this.CreateTablesAsync(new Type[] { typeof(T), typeof(T2), typeof(T3) });
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3, T4>()
		where T : new()
		where T2 : new()
		where T3 : new()
		where T4 : new()
		{
			return this.CreateTablesAsync(new Type[] { typeof(T), typeof(T2), typeof(T3), typeof(T4) });
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3, T4, T5>()
		where T : new()
		where T2 : new()
		where T3 : new()
		where T4 : new()
		where T5 : new()
		{
			return this.CreateTablesAsync(new Type[] { typeof(T), typeof(T2), typeof(T3), typeof(T4), typeof(T5) });
		}

		public Task<CreateTablesResult> CreateTablesAsync(params Type[] types)
		{
			return Task.Factory.StartNew<CreateTablesResult>(() => {
				CreateTablesResult createTablesResult = new CreateTablesResult();
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					Type[] typeArray = types;
					for (int i = 0; i < (int)typeArray.Length; i++)
					{
						Type type = typeArray[i];
						int num = connection.CreateTable(type, CreateFlags.None);
						createTablesResult.Results[type] = num;
					}
				}
				return createTablesResult;
			});
		}

		public Task<int> DeleteAsync(object item)
		{
			return Task.Factory.StartNew<int>(() => {
				int num;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					num = connection.Delete(item);
				}
				return num;
			});
		}

		public Task<int> DropTableAsync<T>()
		where T : new()
		{
			return Task.Factory.StartNew<int>(() => {
				int num;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					num = connection.DropTable<T>();
				}
				return num;
			});
		}

		public Task<int> ExecuteAsync(string query, params object[] args)
		{
			return Task<int>.Factory.StartNew(() => {
				int num;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					num = connection.Execute(query, args);
				}
				return num;
			});
		}

		public Task<T> ExecuteScalarAsync<T>(string sql, params object[] args)
		{
			return Task<T>.Factory.StartNew(() => {
				T t;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					t = connection.CreateCommand(sql, args).ExecuteScalar<T>();
				}
				return t;
			});
		}

		public Task<T> FindAsync<T>(object pk)
		where T : new()
		{
			return Task.Factory.StartNew<T>(() => {
				T t;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					t = connection.Find<T>(pk);
				}
				return t;
			});
		}

		public Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate)
		where T : new()
		{
			return Task.Factory.StartNew<T>(() => {
				T t;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					t = connection.Find<T>(predicate);
				}
				return t;
			});
		}

		public Task<T> GetAsync<T>(object pk)
		where T : new()
		{
			return Task.Factory.StartNew<T>(() => {
				T t;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					t = connection.Get<T>(pk);
				}
				return t;
			});
		}

		public Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
		where T : new()
		{
			return Task.Factory.StartNew<T>(() => {
				T t;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					t = connection.Get<T>(predicate);
				}
				return t;
			});
		}

		private SQLiteConnectionWithLock GetConnection()
		{
			return SQLiteConnectionPool.Shared.GetConnection(this._connectionString, this._openFlags);
		}

		public Task<int> InsertAllAsync(IEnumerable items)
		{
			return Task.Factory.StartNew<int>(() => {
				int num;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					num = connection.InsertAll(items);
				}
				return num;
			});
		}

		public Task<int> InsertAsync(object item)
		{
			return Task.Factory.StartNew<int>(() => {
				int num;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					num = connection.Insert(item);
				}
				return num;
			});
		}

		public Task<List<T>> QueryAsync<T>(string sql, params object[] args)
		where T : new()
		{
			return Task<List<T>>.Factory.StartNew(() => {
				List<T> ts;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					ts = connection.Query<T>(sql, args);
				}
				return ts;
			});
		}

		[Obsolete("Will cause a deadlock if any call in action ends up in a different thread. Use RunInTransactionAsync(Action<SQLiteConnection>) instead.")]
		public Task RunInTransactionAsync(Action<SQLiteAsyncConnection> action)
		{
			return Task.Factory.StartNew(() => {
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					connection.BeginTransaction();
					try
					{
						action(this);
						connection.Commit();
					}
					catch (Exception exception)
					{
						connection.Rollback();
						throw;
					}
				}
			});
		}

		public Task RunInTransactionAsync(Action<SQLiteConnection> action)
		{
			return Task.Factory.StartNew(() => {
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					connection.BeginTransaction();
					try
					{
						action(connection);
						connection.Commit();
					}
					catch (Exception exception)
					{
						connection.Rollback();
						throw;
					}
				}
			});
		}

		public AsyncTableQuery<T> Table<T>()
		where T : new()
		{
			return new AsyncTableQuery<T>(this.GetConnection().Table<T>());
		}

		public Task<int> UpdateAllAsync(IEnumerable items)
		{
			return Task.Factory.StartNew<int>(() => {
				int num;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					num = connection.UpdateAll(items);
				}
				return num;
			});
		}

		public Task<int> UpdateAsync(object item)
		{
			return Task.Factory.StartNew<int>(() => {
				int num;
				SQLiteConnectionWithLock connection = this.GetConnection();
				using (IDisposable disposable = connection.Lock())
				{
					num = connection.Update(item);
				}
				return num;
			});
		}
	}
}