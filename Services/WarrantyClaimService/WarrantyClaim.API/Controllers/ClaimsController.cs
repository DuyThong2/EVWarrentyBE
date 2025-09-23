using BuildingBlocks.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WarrantyClaim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var pagination = new PaginationRequest(pageIndex, pageSize);
            return Ok(pagination);
        }
    }
}
