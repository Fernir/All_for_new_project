using SQLite;
using System;
using System.Runtime.CompilerServices;

namespace HighVoltz.HBRelog.Source.DB
{
	[Table("Accounts")]
	public class Account
	{
		public const string STATUS_OK = "OK";

		public const string STATUS_BANNED = "BANNED";

		public const string STATUS_LOCKED = "LOCKED";

		public const string STATUS_MUTED = "MUTED";

		public const string STATUS_SUSPENDED = "SUSPENDED";

		public const string STATUS_EXPIRED = "EXPIRED";

		public const string STATUS_TRIAL = "TRIAL";

		public const string STATUS_WRONG_PASS = "WRONG_PASS";

		[NotNull]
		public string AccountName
		{
			get;
			set;
		}

		[NotNull]
		public string Answer
		{
			get;
			set;
		}

		public DateTime BanDate
		{
			get;
			set;
		}

		[Ignore]
		public bool CanPlay
		{
			get
			{
				if (this.IsLocked || this.IsSuspended || this.IsBanned)
				{
					return false;
				}
				return !this.IsMuted;
			}
		}

		public DateTime CreationDate
		{
			get;
			set;
		}

		[NotNull]
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

		[Ignore]
		public bool IsBanned
		{
			get
			{
				return this.Status == "BANNED";
			}
		}

		[Ignore]
		public bool IsExpired
		{
			get
			{
				return this.Status == "EXPIRED";
			}
		}

		[Ignore]
		public bool IsLocked
		{
			get
			{
				return this.Status == "LOCKED";
			}
		}

		[Ignore]
		public bool IsMuted
		{
			get
			{
				return this.Status == "MUTED";
			}
		}

		[Ignore]
		public bool IsOK
		{
			get
			{
				return this.Status == "OK";
			}
		}

		[Ignore]
		public bool IsRunning
		{
			get;
			set;
		}

		[Ignore]
		public bool IsSuspended
		{
			get
			{
				return this.Status == "SUSPENDED";
			}
		}

		[Ignore]
		public bool IsTrial
		{
			get
			{
				return this.Status == "TRIAL";
			}
		}

		[Ignore]
		public bool IsWrongPass
		{
			get
			{
				return this.Status == "WRONG_PASS";
			}
		}

		public int MuteCount
		{
			get;
			set;
		}

		public DateTime NextCheckDate
		{
			get;
			set;
		}

		[NotNull]
		public string Password
		{
			get;
			set;
		}

		public string PaymentInfo
		{
			get;
			set;
		}

		public string RegIp
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public int TrialCheckCount
		{
			get;
			set;
		}

		public Account()
		{
		}
	}
}