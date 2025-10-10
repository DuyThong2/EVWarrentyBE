namespace PartCatalog.Application.CQRS.Queries.GetPackageByPeriod
{
    public record GetPackageByPeriodQuery(
        DateTime StartDate,
        DateTime EndDate,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<GetPackageByPeriodResult>;
}
