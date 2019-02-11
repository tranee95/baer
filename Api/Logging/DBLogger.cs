using Api.Model;
using Microsoft.Extensions.Logging;
using System;
using Common.Logging;
using System.Collections.Generic;

namespace Api.Logging
{
	public class DBLogger : ILogger
	{
		private string categoryName;
		private Func<string, LogLevel, bool> filter;
		private ApplicationContext context;
		private bool selfException = false;

		public DBLogger(string _categoryName, Func<string, LogLevel, bool> _filter)
		{
			categoryName = _categoryName;
			filter = _filter;
			context = new ApplicationContext();
		}

		public void Log<TState>(LogLevel logLevel, int eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}
			if (selfException)
			{
				selfException = false;
				return;
			}
			selfException = true;
			if (formatter == null)
			{
				throw new ArgumentNullException(nameof(formatter));
			}
			var message = formatter(state, exception);
			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			if (exception != null)
			{
				message += "\n" + exception.ToString();
			}
			try
			{
				context.EventLog.Add(new EventLog
				{
					Message = message,
					EventID = eventId,
					LogLevel = logLevel.ToString(),
					CreatedTime = DateTime.UtcNow
				});
				context.SaveChanges();
				selfException = false;
			}
			catch { }
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return (filter == null || filter(categoryName, logLevel));
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
		{
			if (eventId != 222 && eventId != 0)
				return;
			if (!IsEnabled(logLevel))
			{
				return;
			}
			if (selfException)
			{
				selfException = false;
				return;
			}
			var message = string.Empty;

			selfException = true;
			if (formatter != null)
			{
				message = formatter(state, exception);
			}

			if (exception != null)
			{
				message += "\n" + exception.ToString();
			}
			try
			{
				var stateItem = state as LogStateItem;

				if (stateItem == null)
				{
					context.EventLog.Add(new EventLog
					{
						Message = message,
						EventID = eventId,
						LogLevel = logLevel.ToString(),
						CreatedTime = DateTime.UtcNow
					});
					context.SaveChanges();
				}
				else
				{
					context.EventLog.Add(new EventLog
					{
						Message = stateItem.Message,
						EventID = eventId,
						LogLevel = logLevel.ToString(),
						CreatedTime = DateTime.UtcNow,
						UserId = stateItem.UserId,
						Path = stateItem.Path,
						Ip = stateItem.Ip,
						UserName = stateItem.UserName,
						EventCode = stateItem.EventCode
					});
					context.SaveChanges();
				}
				selfException = false;
			}
			catch { }
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!new List<int>() {
				0,
				LogEventTypes.TestEvent,
			}.Contains(eventId.Id))
				return;

			if (!IsEnabled(logLevel))
			{
				return;
			}
			if (selfException)
			{
				selfException = false;
				return;
			}
			var message = string.Empty;

			selfException = true;
			if (formatter != null)
			{
				message = formatter(state, exception);
			}

			if (exception != null)
			{
				message += "\n" + exception.ToString();
			}
			try
			{
				var stateItem = state as LogStateItem;

				if (stateItem == null)
				{
					context.EventLog.Add(new EventLog
					{
						Message = message,
						EventID = eventId.Id,
						LogLevel = logLevel.ToString(),
						CreatedTime = DateTime.Now
					});
					context.SaveChanges();
				}
				else
				{
					context.EventLog.Add(new EventLog
					{
						Message = stateItem.Message,
						EventID = eventId.Id,
						LogLevel = logLevel.ToString(),
						CreatedTime = DateTime.Now,
						UserId = stateItem.UserId,
						Path = stateItem.Path,
						Ip = stateItem.Ip,
						UserName = stateItem.UserName,
						EventCode = stateItem.EventCode,
						EventMessage = stateItem.EventMessage,
					});
					context.SaveChanges();
				}
				selfException = false;
			}
			catch { }

		}
	}
}
