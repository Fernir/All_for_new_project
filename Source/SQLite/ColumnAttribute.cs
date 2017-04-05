using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public ColumnAttribute(string name)
		{
			this.Name = name;
		}
	}
}