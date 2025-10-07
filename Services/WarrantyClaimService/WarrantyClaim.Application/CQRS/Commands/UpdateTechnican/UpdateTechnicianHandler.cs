using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateTechnican
{
    internal class UpdateTechnicianHandler
        : ICommandHandler<UpdateTechnicianCommand, UpdateTechnicianResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTechnicianHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateTechnicianResult> Handle(
            UpdateTechnicianCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Technician;

            // Load entity
            var tech = await _context.Technicians
                .FirstOrDefaultAsync(t => t.Id == dto.Id, cancellationToken);

            if (tech is null)
                throw new KeyNotFoundException($"Technician {dto.Id} not found.");

            // Update scalar
            tech.StaffId = dto.StaffId;
            tech.FullName = dto.FullName; // đã được validator đảm bảo not empty
            tech.Email = dto.Email;
            tech.Phone = dto.Phone;
            tech.Status  = dto.Status;

            // Parse Status string -> enum (nếu null/rỗng thì giữ nguyên)
            //if (!string.IsNullOrWhiteSpace(dto.Status) &&
            //    Enum.TryParse<TechnicianStatus>(dto.Status, true, out var parsed))
            //{
            //    tech.Status = parsed;
            //}

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateTechnicianResult(true);
        }
    }
}
