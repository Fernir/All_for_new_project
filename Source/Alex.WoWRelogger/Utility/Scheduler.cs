using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Alex.WoWRelogger.Utility
{
	internal class Scheduler
	{
		public Scheduler()
		{
		}

		public static List<ScheduleUnit> ReadCommands()
		{
			List<ScheduleUnit> scheduleUnits = new List<ScheduleUnit>();
			if (File.Exists("commands.txt"))
			{
				string[] strArrays = File.ReadAllLines("commands.txt");
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str = strArrays[i];
					string[] strArrays1 = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < (int)strArrays1.Length; j++)
					{
						strArrays1[j] = strArrays1[j].Trim();
					}
					if ((int)strArrays1.Length == 2)
					{
						ScheduleUnit scheduleUnit = new ScheduleUnit();
						bool flag = false;
						string[] strArrays2 = strArrays1;
						for (int k = 0; k < (int)strArrays2.Length; k++)
						{
							string str1 = strArrays2[k];
							if (str1.Length < 2 || str1[0] != '\"' || str1[str1.Length - 1] != '\"')
							{
								Log.Err("Cannot read word {0}", new object[] { str1 });
								flag = true;
							}
						}
						if (!flag)
						{
							Match match = (new Regex("^\"(\\d+):(\\d+)\"$")).Match(strArrays1[0]);
							if (!match.Success)
							{
								Log.Err("Wrong time format: {0}", new object[] { strArrays1[0] });
							}
							else
							{
								scheduleUnit.Hour = int.Parse(match.Groups[1].Value);
								scheduleUnit.Minute = int.Parse(match.Groups[2].Value);
								scheduleUnit.Command = strArrays1[1].Substring(1, strArrays1[1].Length - 2);
								scheduleUnits.Add(scheduleUnit);
								Log.Write("Command {0} is loaded", new object[] { str });
							}
						}
					}
					else
					{
						Log.Err("Cannot read line {0}", new object[] { str });
					}
				}
			}
			Log.Err("File {0} not found!", new object[] { "commands.txt" });
			return scheduleUnits;
		}

		private static bool RunCommand(string fullCommand)
		{
			Log.Write("Running command: {0}", new object[] { fullCommand });
			string[] strArrays = fullCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string[] strArrays1 = strArrays[i].Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				if ((int)strArrays1.Length <= 1)
				{
					return false;
				}
				string str = strArrays1[0];
				string str1 = strArrays1[1];
				if (str == "start" && str1 == "all")
				{
					HbRelogManager.StartAllActive();
				}
				else if (!(str == "stop") || !(str1 == "all"))
				{
					CharacterProfile characterProfile = (
						from a in HbRelogManager.CharactersById
						where a.Settings.ProfileName == str1
						select a).FirstOrDefault<CharacterProfile>();
					if (characterProfile == null)
					{
						Log.Err("Profile not found: {0}", new object[] { str1 });
						return false;
					}
					if (str != "start")
					{
						if (str != "stop")
						{
							Log.Err("Action not found: {0}", new object[] { str });
							return false;
						}
						HbRelogManager.StopProfile(characterProfile);
					}
					else
					{
						HbRelogManager.StartProfile(characterProfile);
					}
				}
				else
				{
					HbRelogManager.StopAllActive();
				}
			}
			return true;
		}

		public static void ScheduleThreadStart(object obj)
		{
			List<ScheduleUnit> scheduleUnits = obj as List<ScheduleUnit>;
			if (scheduleUnits == null)
			{
				return;
			}
			foreach (ScheduleUnit scheduleUnit in scheduleUnits)
			{
				DateTime now = DateTime.Now;
				if (scheduleUnit.Hour != now.Hour || scheduleUnit.Minute != now.Minute || scheduleUnit.IsUsedInThatMinute)
				{
					if (scheduleUnit.Hour == now.Hour && scheduleUnit.Minute == now.Minute)
					{
						continue;
					}
					scheduleUnit.IsUsedInThatMinute = false;
				}
				else
				{
					if (!Scheduler.RunCommand(scheduleUnit.Command))
					{
						Log.Err(string.Concat("Wrong command: ", scheduleUnit.Command), new object[0]);
					}
					scheduleUnit.IsUsedInThatMinute = true;
				}
			}
		}
	}
}