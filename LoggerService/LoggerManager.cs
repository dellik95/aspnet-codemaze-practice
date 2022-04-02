﻿using Contracts;
using NLog;
using NLog.Fluent;

namespace LoggerService
{
	public class LoggerManager : ILoggerManager
	{
		private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public void LogInfo(string message)
		{
			_logger.Info(message);
		}

		public void LogWarn(string message)
		{
			_logger.Warn(message);
		}

		public void LogError(string message)
		{
			_logger.Error(message);
		}

		public void LogDebug(string message)
		{
			_logger.Debug(message);
		}
	}
}