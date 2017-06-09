using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PasswordController : Controller
    {
        private readonly ILogger<TestController> logger;
        private readonly UserContext context;
        private readonly Crypto crypto;

        public PasswordController(ILogger<TestController> logger, UserContext context, Crypto crypto)
        {
            this.logger = logger;
            this.context = context;
            this.crypto = crypto;
        }

        [HttpPost]
        [Route("change")]
        public async Task<ActionResult> Change([FromBody]ChangePassword body)
        {
            if (string.IsNullOrEmpty(body.newPassword))
                return BadRequest();

            logger.LogDebug($"User {User.Identity.Name} is changing his password.");

            var user = await context.Users.SingleOrDefaultAsync(u => u.Username == User.Identity.Name);
            if (user == null)
                throw new Exception("You are logged in, but we can't find your user.");

            if (!crypto.Verify(user.Password, body.oldPassword))
                return BadRequest();
            
            user.Password = crypto.Encrypt(body.newPassword);
            user.AskNewPasswordOnNextLogin = false;

            await context.SaveChangesAsync();

            return Ok();
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ChangePassword
    {
        public string oldPassword { get; set; }
        
        public string newPassword { get; set; }
    }
}
