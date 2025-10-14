using EVWUser.Data.Models;

namespace EVWUser.Data.Repositories.Impl
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
