using BuildingBlocks.CQRS;
using AutoMapper;
using PartCatalog.Application.Data;
using PartCatalog.Domain.Models;

namespace PartCatalog.Application.CQRS.Commands.CreatePart
{
    public class CreatePartHandler
        : ICommandHandler<CreatePartCommand, CreatePartResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreatePartHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreatePartResult> Handle(CreatePartCommand request, CancellationToken cancellationToken)
        {
            // Map DTO -> Entity
            var part = _mapper.Map<Part>(request.Part);
            //part.PartId = Guid.NewGuid();

            // Add vào DbContext
            _context.Parts.Add(part);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreatePartResult(part.PartId, true, "Part created successfully.");
        }
    }
}
