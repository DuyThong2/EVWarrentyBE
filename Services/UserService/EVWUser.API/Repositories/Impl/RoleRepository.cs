using EVWUser.API.Data;
using EVWUser.API.Models;

namespace EVWUser.API.Repositories.Impl
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository  
    {
        private readonly UserDbContext _context;
        public RoleRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
