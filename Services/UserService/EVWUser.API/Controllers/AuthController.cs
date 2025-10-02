using BuildingBlocks.CQRS;
using EVWUser.API.Dtos;
using EVWUser.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EVWUser.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// User login
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.Login(request);
            return Ok(ApiResponse<LoginResponse>.Ok(result, "Login successfully"));
        }

        /// <summary>
        /// Extract user info from a JWT access token
        /// </summary>
        /// <param name="token">The JWT token string</param>
        /// <returns>UserDto if token is valid</returns>
        [HttpPost("extract-token")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> ExtractToken([FromBody] string token)
        {
            var user = await _authService.ExtractTokenAsync(token);
            return Ok(ApiResponse<UserDto>.Ok(user, "Extract token successfully"));
        }
    }
}
