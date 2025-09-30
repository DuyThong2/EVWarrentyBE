using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetTechnicianById
{
    public record GetTechnicianByIdQuery(Guid TechnicianId)
        : IQuery<GetTechnicianByIdResult>;

    public record GetTechnicianByIdResult(TechnicianDto Technician);
}
