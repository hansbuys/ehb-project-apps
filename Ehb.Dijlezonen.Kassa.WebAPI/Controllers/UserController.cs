using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserContext context;
        private readonly Crypto crypto;

        public UserController(UserContext context, Crypto crypto)
        {
            this.context = context;
            this.crypto = crypto;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]UserRegistration userRegistration)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username =  userRegistration.Username,
                    Password = crypto.Encrypt(userRegistration.Password),
                    Firstname =  userRegistration.Firstname,
                    Lastname = userRegistration.Lastname,
                    IsBlocked = userRegistration.IsBlocked,
                    AskNewPasswordOnNextLogin = userRegistration.AskNewPasswordOnNextLogin,
                    Roles = userRegistration.Roles.Select(x => context.Roles.Single(r => r.Name == x.Name))
                };

                if (context.Users.Any(u => u.Username == user.Username))
                    return BadRequest("User already exists");
                
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }
    }

    public class UserRegistration
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }


        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public bool AskNewPasswordOnNextLogin { get; set; }
        public bool IsBlocked { get; set; }

        public IEnumerable<RoleRegistration> Roles { get; set; }
    }

    public class RoleRegistration
    {
        public string Name { get; set; }
    }
}