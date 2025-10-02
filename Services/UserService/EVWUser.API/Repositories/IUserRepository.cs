using BuildingBlocks.Pagination;
using EVWUser.API.Models;

namespace EVWUser.API.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<PaginatedResult<User>> GetPagedAsync(PaginationRequest request);
        Task<PaginatedResult<User>> SearchAsync(Guid? roleId, string? email, PaginationRequest request);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task SoftDeleteAsync(Guid id);
    }
}
