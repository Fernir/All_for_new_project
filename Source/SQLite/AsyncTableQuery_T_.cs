using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SQLite
{
	public class AsyncTableQuery<T>
	where T : new()
	{
		private TableQuery<T> _innerQuery;

		public AsyncTableQuery(TableQuery<T> innerQuery)
		{
			this._innerQuery = innerQuery;
		}

		public Task<int> CountAsync()
		{
			return Task.Factory.StartNew<int>(() => {
				int num;
				using (IDisposable disposable = ((SQLiteConnectionWithLock)this._innerQuery.Connection).Lock())
				{
					num = this._innerQuery.Count();
				}
				return num;
			});
		}

		public Task<T> ElementAtAsync(int index)
		{
			return Task.Factory.StartNew<T>(() => {
				T t;
				using (IDisposable disposable = ((SQLiteConnectionWithLock)this._innerQuery.Connection).Lock())
				{
					t = this._innerQuery.ElementAt(index);
				}
				return t;
			});
		}

		public Task<T> FirstAsync()
		{
			return Task<T>.Factory.StartNew(() => {
				T t;
				using (IDisposable disposable = ((SQLiteConnectionWithLock)this._innerQuery.Connection).Lock())
				{
					t = this._innerQuery.First();
				}
				return t;
			});
		}

		public Task<T> FirstOrDefaultAsync()
		{
			return Task<T>.Factory.StartNew(() => {
				T t;
				using (IDisposable disposable = ((SQLiteConnectionWithLock)this._innerQuery.Connection).Lock())
				{
					t = this._innerQuery.FirstOrDefault();
				}
				return t;
			});
		}

		public AsyncTableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr)
		{
			return new AsyncTableQuery<T>(this._innerQuery.OrderBy<U>(orderExpr));
		}

		public AsyncTableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr)
		{
			return new AsyncTableQuery<T>(this._innerQuery.OrderByDescending<U>(orderExpr));
		}

		public AsyncTableQuery<T> Skip(int n)
		{
			return new AsyncTableQuery<T>(this._innerQuery.Skip(n));
		}

		public AsyncTableQuery<T> Take(int n)
		{
			return new AsyncTableQuery<T>(this._innerQuery.Take(n));
		}

		public Task<List<T>> ToListAsync()
		{
			return Task.Factory.StartNew<List<T>>(() => {
				List<T> list;
				using (IDisposable disposable = ((SQLiteConnectionWithLock)this._innerQuery.Connection).Lock())
				{
					list = this._innerQuery.ToList<T>();
				}
				return list;
			});
		}

		public AsyncTableQuery<T> Where(Expression<Func<T, bool>> predExpr)
		{
			return new AsyncTableQuery<T>(this._innerQuery.Where(predExpr));
		}
	}
}