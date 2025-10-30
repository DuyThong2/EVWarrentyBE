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
        private readonly IMapper _mapper;

        public CreateTechnicianHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateTechnicianResult> Handle(
            CreateTechnicianCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Technician;

            var technician = _mapper.Map<Technician>(dto);
            technician.Id = Guid.NewGuid();

            _context.Technicians.Add(technician);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateTechnicianResult(technician.Id);
        }
    
}
}
