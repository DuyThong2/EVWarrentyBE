using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateClaim
{
    public class CreateClaimsHandler : ICommandHandler<CreateClaimsCommand,CreateClaimsResult>
    {
       
        public Task<CreateClaimsResult> Handle(CreateClaimsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
