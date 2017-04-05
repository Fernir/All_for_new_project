using System;
using System.Runtime.CompilerServices;

namespace SQLite
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MaxLengthAttribute : Attribute
	{
		public int Value
		{
			get;
			private set;
		}

		public MaxLengthAttribute(int length)
		{
			this.Value = length;
		}
	}
}