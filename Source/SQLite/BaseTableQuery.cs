using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	public abstract class BaseTableQuery
	{
		protected BaseTableQuery()
		{
		}

		protected class Ordering
		{
			public bool Ascending
			{
				get;
				set;
			}

			public string ColumnName
			{
				get;
				set;
			}

			public Ordering()
			{
			}
		}
	}
}