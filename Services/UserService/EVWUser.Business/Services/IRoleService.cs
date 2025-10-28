using EVWUser.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVWUser.Business.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();
    }
}
