using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Configuration;
using TodoApp.Models.Dto.Requests;
using TodoApp.Models.Dto.Responses;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthManagerService authManagerService;


        public AuthManagementController(UserManager<IdentityUser> userManager, AuthManagerService authManagerService)
        {
            _userManager = userManager;
            this.authManagerService = authManagerService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userInfo)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(userInfo.Email);
                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>
                        {
                            "Пользователся с таким Email уже существует"
                        },
                        Success = false
                    });
                }
                var newUser = new IdentityUser() { UserName = userInfo.UserName, Email = userInfo.Email };
                var isCreated = await _userManager.CreateAsync(newUser, userInfo.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = authManagerService.GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponse()
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }

            }
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>
                {
                    "Неправильный email или пароль"
                },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>
                    {
                    "Пользователся с таким Email не существует"
                    },
                        Success = false
                    });
                }
                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
                if(isCorrect)
                {
                    var jwtToken = authManagerService.GenerateJwtToken(existingUser);
                    return Ok(new RegistrationResponse()
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>
                    {
                    "Неправильный email или пароль"
                    },
                    Success = false
                });
            }
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>
                {
                    "Неккоректный email или пароль"
                },
                Success = false
            });
        }
    }
}
