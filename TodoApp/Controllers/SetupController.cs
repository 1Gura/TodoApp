using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Context;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly ApiDbContext apiDbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<SetupController> logger;
        public SetupController(
            ApiDbContext apiDbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<SetupController> logger
            )
        {
            this.logger = logger;
            this.apiDbContext = apiDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string name)
        {
            var roleExists = await roleManager.RoleExistsAsync(name);
            if (!roleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(name));
                if (roleResult.Succeeded)
                {
                    var successMessage = $"Роль {name} была успешно добавлена";
                    logger.LogInformation(successMessage);
                    return Ok(new
                    {
                        result = successMessage
                    });
                }
                else
                {
                    var errorMessage = $"Роль {name} не была добавлена";
                    logger.LogInformation(errorMessage);
                    return BadRequest(new
                    {
                        error = errorMessage
                    });
                }
            }
            return BadRequest(new { error = "Роль уже существует" });
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
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
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var errorMessage = $"Роль {roleName} не была найдена";
                logger.LogInformation(errorMessage);
                return BadRequest(new
                {
                    error = errorMessage
                });
            }
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new { result = "Пользоватль был добавлен в роль" });
            }
            else
            {
                var errorMessage = $"Пользователю не была добавлена роль {roleName}";
                logger.LogInformation(errorMessage);
                return BadRequest(new
                {
                    error = errorMessage
                });
            }
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
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

            var roles = await userManager.GetRolesAsync(user);
            return Ok(roles);
        }
        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
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
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var errorMessage = $"Роль {roleName} не была найдена";
                logger.LogInformation(errorMessage);
                return BadRequest(new
                {
                    error = errorMessage
                });
            }
            var result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new { result = $"Пользователь {user} был удален из роли {roleName}" });
            }

            return BadRequest(new { error = $"Не удалось удалить пользователя {user}  из роли {roleName}" });
        }

    }
}
