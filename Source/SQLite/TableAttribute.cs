using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public TableAttribute(string name)
		{
			this.Name = name;
		}
	}
}