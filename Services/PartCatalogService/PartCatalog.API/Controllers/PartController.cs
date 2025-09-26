using BuildingBlocks.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace PartCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var pagination = new PaginationRequest(pageIndex, pageSize);
            return Ok(pagination);
        }
    }
}
