using SQLite;
using System;
using System.Runtime.CompilerServices;

namespace HighVoltz.HBRelog.Source
{
	[Table("Messages")]
	public class Message
	{
		public string AccountName
		{
			get;
			set;
		}

		public string Character
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		[AutoIncrement]
		[PrimaryKey]
		[Unique]
		public int Id
		{
			get;
			set;
		}

		public string Server
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public Message()
		{
		}
	}
}