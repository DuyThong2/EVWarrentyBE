using BuildingBlocks.CQRS;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Commands.CreatePart
{
    // Command nhận dữ liệu từ client
    public record CreatePartCommand(CreatePartDto Part)
        : ICommand<CreatePartResult>;

    // Kết quả trả về
    public record CreatePartResult(Guid PartId, bool IsSuccess, string Message);
}
