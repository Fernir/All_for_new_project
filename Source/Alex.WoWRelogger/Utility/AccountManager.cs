using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using HighVoltz.HBRelog.Source;
using HighVoltz.HBRelog.Source.DB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Alex.WoWRelogger.Utility
{
	internal static class AccountManager
	{
		private static List<Account> Accounts;

		public const string DB_NAME = "Accounts.sqlite";

		private static Timer tmr;

		private readonly static DateTime year2099;

		private readonly static DateTime year2000;

		private static object lockObj;

		public const int MAX_MUTE_COUNT = 5;

		static AccountManager()
		{
			AccountManager.Accounts = new List<Account>();
			AccountManager.year2099 = DateTime.Parse("2099-01-01 00:00:00");
			AccountManager.year2000 = DateTime.Parse("2000-10-10");
			AccountManager.lockObj = new object();
		}

		public static void AddAccount(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Accounts.sqlite", false))
				{
					AccountManager.Accounts.Add(acc);
					sQLiteConnection.Insert(acc);
				}
			}
		}

		public static void AddMessage(Message m)
		{
			lock (AccountManager.lockObj)
			{
				using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Accounts.sqlite", false))
				{
					sQLiteConnection.Insert(m);
				}
			}
		}

		public static void Ban(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				acc.Status = "BANNED";
				acc.BanDate = DateTime.Now;
				AccountManager.UpdateAccount(acc);
			}
		}

		public static void CreateRecheckTimer()
		{
			AccountManager.tmr = new Timer(new TimerCallback(AccountManager.RecheckPulse), "tick...", 1000, 5000);
		}

		public static void Expired(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				acc.Status = "EXPIRED";
				acc.BanDate = DateTime.Now;
				AccountManager.UpdateAccount(acc);
			}
		}

		public static Account GetAccountByEmail(string email)
		{
			Account account;
			lock (AccountManager.lockObj)
			{
				Account account1 = AccountManager.Accounts.Where<Account>((Account a) => {
					if (!(a.Email == email) || a.IsBanned || a.IsExpired || a.IsLocked || a.IsSuspended || a.IsRunning || a.IsMuted)
					{
						return false;
					}
					if (!a.IsTrial)
					{
						return true;
					}
					if (!a.IsTrial)
					{
						return false;
					}
					return HbRelogManager.Settings.AllowTrials;
				}).FirstOrDefault<Account>();
				if (account1 == null)
				{
					return null;
				}
				else
				{
					account1.IsRunning = true;
					account = account1;
				}
			}
			return account;
		}

		public static Account GetAccountById(int Id)
		{
			Account account;
			lock (AccountManager.lockObj)
			{
				Account account1 = AccountManager.Accounts.Where<Account>((Account a) => {
					if (a.Id != Id || a.IsBanned || a.IsExpired || a.IsLocked || a.IsSuspended || a.IsRunning || a.IsMuted)
					{
						return false;
					}
					if (!a.IsTrial)
					{
						return true;
					}
					if (!a.IsTrial)
					{
						return false;
					}
					return HbRelogManager.Settings.AllowTrials;
				}).FirstOrDefault<Account>();
				if (account1 == null)
				{
					return null;
				}
				else
				{
					account1.IsRunning = true;
					account = account1;
				}
			}
			return account;
		}

		public static List<Account> GetAccounts()
		{
			List<Account> accounts;
			lock (AccountManager.lockObj)
			{
				accounts = AccountManager.Accounts;
			}
			return accounts;
		}

		public static Account GetFreeAccount()
		{
			Account account;
			lock (AccountManager.lockObj)
			{
				Account account1 = AccountManager.Accounts.Where<Account>((Account a) => {
					if (a.IsRunning)
					{
						return false;
					}
					if (a.IsOK)
					{
						return true;
					}
					if (!a.IsTrial)
					{
						return false;
					}
					return HbRelogManager.Settings.AllowTrials;
				}).FirstOrDefault<Account>();
				if (account1 == null)
				{
					return null;
				}
				else
				{
					account1.IsRunning = true;
					account = account1;
				}
			}
			return account;
		}

		public static bool HaveActivePaidNotRunningAccounts()
		{
			bool flag;
			lock (AccountManager.lockObj)
			{
				if (AccountManager.Accounts.Where<Account>((Account a) => {
					if (a.IsRunning)
					{
						return false;
					}
					if (a.IsLocked || a.IsOK || a.IsTrial && HbRelogManager.Settings.AllowTrials)
					{
						return true;
					}
					if (!a.IsTrial)
					{
						return false;
					}
					return !string.IsNullOrEmpty(a.PaymentInfo);
				}).FirstOrDefault<Account>() == null)
				{
					return false;
				}
				else
				{
					flag = true;
				}
			}
			return flag;
		}

		public static void Lock(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				acc.Status = "LOCKED";
				acc.NextCheckDate = DateTime.Now + new TimeSpan(0, 15, 0);
				AccountManager.UpdateAccount(acc);
			}
		}

		public static void Mute(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				Account muteCount = acc;
				muteCount.MuteCount = muteCount.MuteCount + 1;
				acc.Status = "MUTED";
				acc.NextCheckDate = DateTime.Now + new TimeSpan((int)Math.Pow(2, (double)(acc.MuteCount - 1)), 0, 0, 0);
				if (acc.MuteCount > 5)
				{
					AccountManager.Ban(acc);
				}
				AccountManager.UpdateAccount(acc);
			}
		}

		private static void RecheckPulse(object data)
		{
			DateTime now = DateTime.Now;
			lock (AccountManager.lockObj)
			{
				foreach (Account account in AccountManager.Accounts.Where<Account>((Account a) => {
					if (!a.IsMuted && !a.IsSuspended && !a.IsLocked && !a.IsTrial || a.IsBanned || a.IsExpired || a.IsRunning)
					{
						return false;
					}
					return now > a.NextCheckDate;
				}))
				{
					if (account.NextCheckDate < AccountManager.year2000)
					{
						account.NextCheckDate = AccountManager.year2099;
					}
					account.Status = "OK";
					AccountManager.UpdateAccount(account);
				}
			}
		}

		public static void Reload()
		{
			List<Account> list = null;
			using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Accounts.sqlite", false))
			{
				list = sQLiteConnection.Table<Account>().ToList<Account>();
			}
			lock (AccountManager.lockObj)
			{
				foreach (Account account in list)
				{
					Account email = (
						from a in AccountManager.Accounts
						where a.Id == account.Id
						select a).FirstOrDefault<Account>();
					if (email == null)
					{
						AccountManager.Accounts.Add(account);
					}
					else
					{
						email.Email = account.Email;
						email.Password = account.Password;
						email.Answer = account.Answer;
						email.AccountName = account.AccountName;
						email.Status = account.Status;
						email.MuteCount = account.MuteCount;
						email.CreationDate = account.CreationDate;
						email.BanDate = account.BanDate;
						email.NextCheckDate = account.NextCheckDate;
						email.PaymentInfo = account.PaymentInfo;
						email.TrialCheckCount = account.TrialCheckCount;
					}
				}
			}
		}

		public static void Suspend(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				acc.Status = "SUSPENDED";
				acc.NextCheckDate = DateTime.Now + new TimeSpan(4, 0, 0);
				AccountManager.UpdateAccount(acc);
			}
		}

		public static void Trial(Account acc, bool increaseCount = false)
		{
			lock (AccountManager.lockObj)
			{
				acc.Status = "TRIAL";
				if (increaseCount)
				{
					Account trialCheckCount = acc;
					trialCheckCount.TrialCheckCount = trialCheckCount.TrialCheckCount + 1;
				}
				if (acc.TrialCheckCount > 10)
				{
					acc.NextCheckDate = DateTime.Parse("2099-01-01 00:00:00");
				}
				else
				{
					acc.NextCheckDate = DateTime.Now + new TimeSpan(0, 5, 0);
				}
				AccountManager.UpdateAccount(acc);
			}
		}

		public static void UpdateAccount(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Accounts.sqlite", false))
				{
					sQLiteConnection.Update(acc);
				}
			}
		}

		public static void WrongPass(Account acc)
		{
			lock (AccountManager.lockObj)
			{
				acc.Status = "WRONG_PASS";
				acc.NextCheckDate = AccountManager.year2099;
				AccountManager.UpdateAccount(acc);
			}
		}
	}
}