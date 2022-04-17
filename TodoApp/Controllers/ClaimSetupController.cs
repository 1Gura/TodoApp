using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.Context;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimSetupController : ControllerBase
    {
        private readonly ApiDbContext apiDbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<ClaimSetupController> logger;
        public ClaimSetupController(
            ApiDbContext apiDbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<ClaimSetupController> logger
            )
        {
            this.logger = logger;
            this.apiDbContext = apiDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClaims(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var errorMessage = $"Пользователь с email {email} не был найден";
                logger.LogInformation(errorMessage);
                return BadRequest(new
                {
                    error = errorMessage
                });
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            return Ok(userClaims);
        }

        [HttpPost]
        [Route("AddClaimsToUser")]
        public async Task<IActionResult> AddClaimsToUser(string email, string claimName, string claimValue)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var errorMessage = $"Пользователь с email {email} не был найден";
                logger.LogInformation(errorMessage);
                return BadRequest(new
                {
                    error = errorMessage
                });
            }

            var userClaim = new Claim(claimName, claimValue);
            var result = await userManager.AddClaimAsync(user, userClaim);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"У пользователя {user.Email} появился claim {claimName}"
                });
            }
            return BadRequest(new
            {
                result = $"Не удалось добавить claim ${claimName} пользователю {user.Email}"
            });
        }
    }
}
