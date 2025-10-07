using BuildingBlocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.Exceptions
{
    internal class ClaimNotFoundException : NotFoundException
    {
        public ClaimNotFoundException(Guid id) : base("Claim", id)
        {
        }
    }
}
