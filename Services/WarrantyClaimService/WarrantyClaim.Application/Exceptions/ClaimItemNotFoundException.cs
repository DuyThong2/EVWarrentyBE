using BuildingBlocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.Exceptions
{
    internal class ClaimItemNotFoundException : NotFoundException
    {
        public ClaimItemNotFoundException(Guid id) : base("Claim item", id)
        {
        }
    }
}
