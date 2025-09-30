using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteTechnican
{
    internal class DeleteTechnicianHandler
        : ICommandHandler<DeleteTechnicianCommand, DeleteTechnicianResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTechnicianHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteTechnicianResult> Handle(
            DeleteTechnicianCommand request,
            CancellationToken cancellationToken)
        {
            // Tìm entity
            var tech = await _context.Technicians.FindAsync([request.TechnicianId], cancellationToken);
            if (tech is null)
                throw new KeyNotFoundException($"Technician {request.TechnicianId} not found.");

            _context.Technicians.Remove(tech);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteTechnicianResult(true);
        }
    }
}
