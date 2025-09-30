using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateTechician
{
    internal class CreateTechnicianHandler
        : ICommandHandler<CreateTechnicianCommand, CreateTechnicianResult>
    {
        private readonly IApplicationDbContext _context;

        public CreateTechnicianHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateTechnicianResult> Handle(
            CreateTechnicianCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Technician;

            // Parse status string -> enum (default ACTIVE)
            var status = dto.Status;
            //if (!string.IsNullOrWhiteSpace(dto.Status) &&
            //    Enum.TryParse<TechnicianStatus>(dto.Status, true, out var parsed))
            //{
            //    status = parsed;
            //}

            var technician = new Technician
            {
                Id = Guid.NewGuid(),
                StaffId = dto.StaffId,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Status = status
            };

            _context.Technicians.Add(technician);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateTechnicianResult(technician.Id);
        }
    }
}
