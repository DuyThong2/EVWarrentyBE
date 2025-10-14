using BuildingBlocks.Authorization;
using BuildingBlocks.Pagination;
using EVWUser.Business.Dtos;
using EVWUser.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVWUser.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(ApiResponse<UserDto>.Ok(user, "User retrieved successfully"));
        }

        /// <summary>
        /// Get users with filter and pagination
        /// </summary>
        [HttpGet]
        [HasRoles("Admin")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<UserDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaged([FromQuery] Guid? roleId, [FromQuery] string? email, [FromQuery] int index, [FromQuery] int pageSize)
        {
            var users = await _userService.SearchAsync(roleId, email, new PaginationRequest(index, pageSize));
            return Ok(ApiResponse<PaginatedResult<UserDto>>.Ok(users, "Users retrieved successfully"));
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult> Create(UserCreateRequest request)
        {
            var created = await _userService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId },
                ApiResponse<UserDto>.Ok(created, "User created successfully"));
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Update(Guid id, UserUpdateRequest request)
        {
            var updated = await _userService.UpdateAsync(id, request);
            return Ok(ApiResponse<UserDto>.Ok(updated, "User updated successfully"));
        }

        /// <summary>
        /// Soft delete or locked a user
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult> SoftDelete(Guid id)
        {
            await _userService.SoftDeleteAsync(id);
            return Ok(ApiResponse<string>.Ok("User locked successfully"));
        }
    }
}
