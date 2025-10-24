using MediatR;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.Commands.CreatePackage
{
    public record CreatePackageCommand(CreatePackageDto Package) : ICommand<CreatePackageResult>;

    public record CreatePackageResult(Guid PackageId, bool Success, string? Message, DateTime? CreatedAt);
}
