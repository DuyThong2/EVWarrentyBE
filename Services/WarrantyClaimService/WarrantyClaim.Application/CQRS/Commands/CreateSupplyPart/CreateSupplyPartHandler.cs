using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Application.CQRS.Commands.CreateSupplyPart
{
    internal class CreateSupplyPartHandler
        : ICommandHandler<CreateSupplyPartCommand, CreateSupplyPartResult>
    {
        private readonly IApplicationDbContext _context;

        public CreateSupplyPartHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateSupplyPartResult> Handle(
            CreateSupplyPartCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.SupplyPart;

            // Parse status string -> enum (default PENDING)
            var status = SupplyStatus.REQUESTED;
            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<SupplyStatus>(dto.Status, true, out var parsed))
            {
                status = parsed;
            }

            var partSupply = new PartSupply
            {
                Id = Guid.NewGuid(),
                ClaimItemId = dto.ClaimItemId,
                PartId = dto.PartId,
                Description = dto.Description,
                NewSerialNumber = dto.NewSerialNumber,
                ShipmentCode = dto.ShipmentCode,
                ShipmentRef = dto.ShipmentRef,
                Status = status
            };

            _context.PartSupplies.Add(partSupply);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateSupplyPartResult(partSupply.Id);


        }
    }
}
