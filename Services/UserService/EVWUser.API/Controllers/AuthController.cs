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

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return Ok(ApiResponse<LoginResult>.Ok(result, "Login successfully"));
        }
    }
}
