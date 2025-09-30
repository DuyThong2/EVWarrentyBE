using BuildingBlocks.Exceptions;
using EVWUser.API.Data;
using EVWUser.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EVWUser.API.Repositories.Impl
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        private readonly UserDbContext _context;

        public UserRoleRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserRole> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId)
        {
            try
            {
                var userRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
                if (userRole == null)
                    throw new NotFoundException("UserRole not found");
                return userRole;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InternalServerException("Error retrieving data");
            }

        }

        public async Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw new InternalServerException("Error retrieving data");
            }
        }

        public async Task<IEnumerable<Guid>> GetRoleIdsByUserIdAsync(Guid userId)
        {
            try
            {
                return await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw new InternalServerException("Error retrieving data");
            }
        }
    }
}
