using Alex.WoWRelogger.Utility;
using HighVoltz.HBRelog.Source.AntiCaptcha;
using System;
using System.Threading;

namespace HighVoltz.HBRelog.Source.Utility
{
	internal static class CaptchaSolver
	{
		private const string Host = "api.anti-captcha.com";

		private const string ClientKey = "1d440974518cc326e790e66fdbcc0b73";

		private static string ProcessTask(AnticaptchaTask task)
		{
			AnticaptchaResult taskResult;
			do
			{
				taskResult = AnticaptchaApiWrapper.GetTaskResult("api.anti-captcha.com", "1d440974518cc326e790e66fdbcc0b73", task);
				if (taskResult == null || taskResult.GetStatus().Equals(AnticaptchaResult.Status.ready))
				{
					break;
				}
				if (!taskResult.GetStatus().Equals(AnticaptchaResult.Status.processing))
				{
					continue;
				}
				Log.Write("Not done yet, waiting...", new object[0]);
				Thread.Sleep(1000);
			}
			while (taskResult.GetStatus().Equals(AnticaptchaResult.Status.processing));
			if (taskResult != null && taskResult.GetSolution() != null)
			{
				Log.Write(string.Concat("The answer is '", taskResult.GetSolution(), "'"), new object[0]);
				return taskResult.GetSolution();
			}
			Log.Write(string.Concat("Unfortunately we got the following error from the API: ", (taskResult != null ? taskResult.GetErrorDescription() : "/empty/")), new object[0]);
			return "";
		}

		public static string Solve(string base64picture)
		{
			bool? nullable = null;
			bool? nullable1 = nullable;
			nullable = null;
			bool? nullable2 = nullable;
			int? nullable3 = null;
			int? nullable4 = nullable3;
			nullable = null;
			nullable3 = null;
			int? nullable5 = nullable3;
			nullable3 = null;
			AnticaptchaTask textTask = AnticaptchaApiWrapper.CreateImageToTextTask("api.anti-captcha.com", "1d440974518cc326e790e66fdbcc0b73", base64picture, nullable1, nullable2, nullable4, nullable, nullable5, nullable3);
			if (textTask.GetErrorDescription() != null && textTask.GetErrorDescription().Length > 0)
			{
				Log.Write(string.Concat("Unfortunately we got the following error from the API: ", textTask.GetErrorDescription()), new object[0]);
			}
			Log.Write(string.Concat("Task ID is ", textTask.GetTaskId(), ". ImageToText task is sent, will wait for the result."), new object[0]);
			Thread.Sleep(2000);
			return CaptchaSolver.ProcessTask(textTask);
		}
	}
}