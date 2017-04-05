using System;

namespace HighVoltz.HBRelog.Source.AntiCaptcha
{
	public class AnticaptchaResult
	{
		private readonly double? _cost;

		private readonly int? _createTime;

		private readonly int? _endTime;

		private readonly string _errorCode;

		private readonly string _errorDescription;

		private readonly int? _errorId;

		private readonly string _ip;

		private readonly string _solution;

		private readonly int? _solveCount;

		private readonly AnticaptchaResult.Status? _status;

		public AnticaptchaResult(AnticaptchaResult.Status? status, string solution, int? errorId, string errorCode, string errorDescription, double? cost, string ip, int? createTime, int? endTime, int? solveCount)
		{
			this._errorId = errorId;
			this._errorCode = errorCode;
			this._errorDescription = errorDescription;
			this._status = status;
			this._solution = solution;
			this._cost = cost;
			this._ip = ip;
			this._createTime = createTime;
			this._endTime = endTime;
			this._solveCount = solveCount;
		}

		public double? GetCost()
		{
			return this._cost;
		}

		public int? GetCreateTime()
		{
			return this._createTime;
		}

		public int? GetEndTime()
		{
			return this._endTime;
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

		public string GetIp()
		{
			return this._ip;
		}

		public string GetSolution()
		{
			return this._solution;
		}

		public int? GetSolveCount()
		{
			return this._solveCount;
		}

		public AnticaptchaResult.Status? GetStatus()
		{
			return this._status;
		}

		public override string ToString()
		{
			return string.Concat(new object[] { "AnticaptchaResult{errorId=", this._errorId, ", errorCode='", this._errorCode, "', errorDescription='", this._errorDescription, "', status=", this._status, ", solution='", this._solution, "', cost=", this._cost, ", ip='", this._ip, "', createTime=", this._createTime, ", endTime=", this._endTime, ", solveCount=", this._solveCount, "}" });
		}

		public enum Status
		{
			ready,
			unknown,
			processing
		}
	}
}