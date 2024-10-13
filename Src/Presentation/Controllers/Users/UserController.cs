using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysgamingApi.Src.Application.Users.Command.LoginUser;
using SysgamingApi.Src.Application.Users.Command.RegisterUser;
using SysgamingApi.Src.Application.Utils;

namespace SysgamingApi.Src.Presentation.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly ILoginUserUseCase _loginUserUseCase;
        private readonly ILoggedInUserService _loggedInUserService;

        public UserController(
        IRegisterUserUseCase registerUserUseCase,
        ILoginUserUseCase loginUserUseCase,
        ILoggedInUserService loggedInUserService
        )
        {
            _registerUserUseCase = registerUserUseCase;
            _loginUserUseCase = loginUserUseCase;
            _loggedInUserService = loggedInUserService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _registerUserUseCase.HandleAsync(request);

            if (response == null)
                return BadRequest();

            return Ok(response);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _loginUserUseCase.HandleAsync(request);

            if (response == null)
                return BadRequest();

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("get-me")]
        public IActionResult GetMe()
        {
            var userId = _loggedInUserService.UserId;
            if (userId == null)
                return BadRequest();

            return Ok(userId);
        }


    }

}