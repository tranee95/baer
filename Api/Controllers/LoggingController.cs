using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Api.Model;
using Common.Logging;

namespace Api.Controllers
{
	public class LoggingController : ControllerBase
	{
		private readonly UserManager<User> userManager;
		private readonly IHttpContextAccessor accessor;
		private readonly ILogger logger;

		public LoggingController(UserManager<User> _userManager, IHttpContextAccessor _accessor, ILogger _logger)
		{
			userManager = _userManager;
			accessor = _accessor;
			logger = _logger;
		}

		public string GetUserId()
		{
			return userManager.GetUserId(User);
		}

		public string GetIpAddress()
		{
			return accessor.HttpContext.Connection.RemoteIpAddress.ToString();
		}

		public void Log(LogLevel logLevel, int eventId, int eventCode, string message, string eventMessage = "")
		{
			logger.Log(LogLevel.Information, eventId,
				new LogStateItem
				{
					Message = message,
					Path = Request.Path.Value,
					Ip = GetIpAddress(),
					UserId = GetUserId(),
					UserName = User.Identity.Name,
					EventCode = eventCode,
					EventMessage = eventMessage
				}, null, (st, cb) => { return string.Empty; });
		}

		public void Log(LogLevel logLevel, int eventId, int eventCode, string message, string userId, string userName, string eventMessage = "")
		{
			logger.Log(LogLevel.Information, eventId,
				new LogStateItem
				{
					Message = message,
					Path = Request.Path.Value,
					Ip = GetIpAddress(),
					UserId = userId,
					UserName = userName,
					EventCode = eventCode,
					EventMessage = eventMessage
				}, null, (st, cb) => { return string.Empty; });
		}
	}
}
