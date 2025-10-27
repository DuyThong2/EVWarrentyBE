using BuildingBlocks.CQRS;
using EVWUser.Business.Dtos;
using EVWUser.Business.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVWUser.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
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
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> ExtractToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return BadRequest(ApiResponse<string>.Fail("Missing or invalid Authorization header"));

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var user = await _authService.ExtractTokenAsync(token);
            return Ok(ApiResponse<UserDto>.Ok(user, "Extract token successfully"));
        }
    }
}
