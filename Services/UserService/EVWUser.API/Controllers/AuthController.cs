using BuildingBlocks.CQRS;
using EVWUser.API.Auth.Login;
using EVWUser.API.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EVWUser.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <description>
        /// Allows a user to log in using their email and password.  
        /// On success, returns an access token, expiry time, and user information.  
        /// </description>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return Ok(ApiResponse<LoginResult>.Ok(result, "Login successfully"));
        }
    }
}
