using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SQLite
{
	public class CreateTablesResult
	{
		public Dictionary<Type, int> Results
		{
			get;
			private set;
		}

		internal CreateTablesResult()
		{
			this.Results = new Dictionary<Type, int>();
		}
	}
}