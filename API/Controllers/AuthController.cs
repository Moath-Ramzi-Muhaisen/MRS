using Appliction.Services.AuthServices;
using Appliction.Services.AuthServices.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto input)
        {
            var result = await _authService.Login(input);
            return Ok(result);
        }
        [HttpPost("Refresh_Token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto input)
        {
            var result = await _authService.RefreshToken(input);

            return Ok(result);
        }
        [HttpPut("Change_User_Password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto input)
        {
            await _authService.ChangeUserPassword(input);
            return Ok();
        }

        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout(Guid id)
        {
            await _authService.Logout(id);
            return Ok();
        }
    }
}
