using BuildingBlocks.CQRS;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using PartCatalog.Application.Data;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.CQRS.Commands.UpdatePart
{
    public class UpdatePartHandler
        : ICommandHandler<UpdatePartCommand, UpdatePartResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePartHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdatePartResult> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
        {
            var part = await _context.Parts
                .FirstOrDefaultAsync(x => x.PartId == request.PartId, cancellationToken);

            if (part == null)
                return new UpdatePartResult(false);

            // Map dữ liệu mới từ DTO vào entity (chỉ các field cho phép update)
            _mapper.Map(request.Part, part);

            _context.Parts.Update(part);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePartResult(true);
        }
    }
}
