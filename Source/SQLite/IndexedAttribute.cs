using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	[AttributeUsage(AttributeTargets.Property)]
	public class IndexedAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}

		public virtual bool Unique
		{
			get;
			set;
		}

		public IndexedAttribute()
		{
		}

		public IndexedAttribute(string name, int order)
		{
			this.Name = name;
			this.Order = order;
		}
	}
}