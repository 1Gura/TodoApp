using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Configuration;
using TodoApp.Context;
using TodoApp.Mapping;
using TodoApp.Models.Dto;
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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AuthManagementController> logger;
        private readonly ApiDbContext context;
        
        private IMapper mapper { get; set; } = null!;
        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            AuthManagerService authManagerService,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthManagementController> logger,
            ApiDbContext context
            )
        {
            _userManager = userManager;
            this.authManagerService = authManagerService;
            this.roleManager = roleManager;
            this.logger = logger;
            this.context = context;
            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile<AppMappingProfile>();
            });
            mapperConfig.AssertConfigurationIsValid();
            mapper = mapperConfig.CreateMapper();
        }

        [HttpGet]
        [Route("UniqEmail")]
        public async Task<ActionResult<bool>> CheckEmailUniq([FromQuery] string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (null == existingUser)
            {
                return Ok(null);
            }

            return Ok(true);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Пользователь с таким Email уже существует"
                            },
                        Success = false
                    });
                }
                if (user.Password != user.RepeatPassword)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                               "Пароли не совпадают"
                            },
                        Success = false
                    });
                }
                var newUser = new IdentityUser() { Email = user.Email, UserName = user.UserName };
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "AppUser");
                    var jwtToken = await authManagerService.GenerateJwtTokenAsync(newUser);
                    return Ok(jwtToken);
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
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [HttpGet]
        [Route("GetUserInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IdentityUser>> UserInfo(string userEmail)
        {
            var user = this.context.Users.FirstOrDefault(x => x.Email == userEmail);
            if(user == null)
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
            var userShort = this.mapper.Map<UserDto>(user);
            return Ok(userShort);
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
                if (isCorrect)
                {
                    return Ok(await authManagerService.GenerateJwtTokenAsync(existingUser));
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



        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await authManagerService.VerifyAndGenerateToken(tokenRequest);
                if (result == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                    {
                        "Invalid token"
                    },
                        Success = false
                    });
                }
                return Ok(result);
            }
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }
    }
}
