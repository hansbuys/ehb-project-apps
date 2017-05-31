using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> logger;

        public TestController(ILogger<TestController> logger)
        {
            this.logger = logger;
        }


        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            logger.LogDebug("Getting values");
            return new[] {"value1", "value2"};
        }
    }
}