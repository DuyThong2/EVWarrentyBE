using BuildingBlocks.Authorization;
using EVWUser.Business.Dtos;
using EVWUser.Business.Services;
using EVWUser.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EVWUser.API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<Role>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(ApiResponse<List<Role>>.Ok(roles.ToList(), "Roles retrieved successfully"));
        }
    }
}
