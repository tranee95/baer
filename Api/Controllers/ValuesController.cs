using System.Collections.Generic;
using Api.Model;
using Microsoft.AspNetCore.Mvc;
using Common;
using Common.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : LoggingController
    {
		private readonly ApplicationContext context;
		private readonly UserManager<User> userManager;
		private readonly ILogger logger;
		private readonly IHttpContextAccessor accessor;

		public ValuesController(ApplicationContext _context,
								ILogger<ValuesController> _logger,
								IHttpContextAccessor _accessor,
								UserManager<User> _userManager
								): base(_userManager, _accessor, _logger)
		{
			context = _context;
			userManager = _userManager;
			logger = _logger;
			accessor = _accessor;
		}

		// GET api/values
		[HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
			context.Test.Add(new Test
			{
				TestStr = "test test"
			});

			context.SaveChanges();

			Log(LogLevel.Information, LogEventTypes.TestEvent, 210, $"test log");

            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
