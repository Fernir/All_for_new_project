using System;

namespace HighVoltz.HBRelog.Source.AntiCaptcha
{
	public class AnticaptchaTask
	{
		private readonly string _errorCode;

		private readonly string _errorDescription;

		private readonly int? _errorId;

		private readonly int? _taskId;

		public AnticaptchaTask(int? taskId, int? errorId, string errorCode, string errorDescription)
		{
			this._errorId = errorId;
			this._taskId = taskId;
			this._errorCode = errorCode;
			this._errorDescription = errorDescription;
		}

		public string GetErrorCode()
		{
			return this._errorCode;
		}

		public string GetErrorDescription()
		{
			return this._errorDescription;
		}

		public int? GetErrorId()
		{
			return this._errorId;
		}

		public int? GetTaskId()
		{
			return this._taskId;
		}

		public override string ToString()
		{
			return string.Concat(new object[] { "AnticaptchaTask{errorId=", this._errorId, ", taskId=", this._taskId, ", errorCode='", this._errorCode, "', errorDescription='", this._errorDescription, "'}" });
		}
	}
}