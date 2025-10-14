using EVWUser.Data.Models;

namespace EVWUser.Data.Repositories
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<UserRole> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId);
        Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Guid>> GetRoleIdsByUserIdAsync(Guid userId);
    }
}
