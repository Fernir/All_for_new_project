using System;
using System.Runtime.CompilerServices;

namespace Alex.WoWRelogger.Utility
{
	internal class ScheduleUnit
	{
		public string Command
		{
			get;
			set;
		}

		public int Hour
		{
			get;
			set;
		}

		public bool IsUsedInThatMinute
		{
			get;
			set;
		}

		public int Minute
		{
			get;
			set;
		}

		public ScheduleUnit()
		{
		}
	}
}