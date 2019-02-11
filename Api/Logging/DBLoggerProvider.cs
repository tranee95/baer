using System;
using Microsoft.Extensions.Logging;

namespace Api.Logging
{
	public class DBLoggerProvider : ILoggerProvider
	{
		private readonly Func<string, LogLevel, bool> _filter;

		public DBLoggerProvider(Func<string, LogLevel, bool> filter)
		{
			_filter = filter;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new DBLogger(categoryName, _filter);
		}

		public void Dispose()
		{

		}
	}

	public static class LoggerProviderExtensions
	{
		public static ILoggerFactory AddDBLogging(this ILoggerFactory loggerFactory,
		  Func<string, LogLevel, bool> filter = null)
		{
			loggerFactory?.AddProvider(new DBLoggerProvider(filter));
			return loggerFactory;
		}
	}
}
