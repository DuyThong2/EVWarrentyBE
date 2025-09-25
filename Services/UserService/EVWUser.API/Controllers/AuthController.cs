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
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand cmd, CancellationToken ct)
        {
            var result = await _mediator.Send(cmd, ct);
            return Ok(result);
        }
    }
}
