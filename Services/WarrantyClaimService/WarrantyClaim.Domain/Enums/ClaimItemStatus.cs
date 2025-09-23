using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Domain.Enums
{
    public enum ClaimItemStatus
    {
        PENDING = 0,
        APPROVED = 1,
        REJECTED = 2,
        IN_PROGRESS = 3,
        COMPLETED = 4
        
    }
}
