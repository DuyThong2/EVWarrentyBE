// WarrantyClaim.Application/CQRS/Queries/GetClaimByFilter/GetClaimsFilteredQuery.cs

using BuildingBlocks.Pagination;

namespace WarrantyClaim.Application.CQRS.Queries.GetClaimByFilter
{
    public record GetClaimsFilteredQuery(
        ClaimsFilter Filter,
        PaginationRequest Pagination,
        SortOption Sort,
        IncludeOption Include
    ) : IQuery<GetClaimsFilteredResult>;

    public record GetClaimsFilteredResult(PaginatedResult<ClaimDto> Result);

    public class ClaimsFilter
    {
        public Guid? Id { get; set; }
        public string? VIN { get; set; }
        public string? Status { get; set; }       // e.g. SUBMITTED
        public string? ClaimType { get; set; }    // e.g. WARRANT

        // Time range + UI chooses field via "dateField" = createdAt | lastModified
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string? DateField { get; set; }    // "createdAt" | "lastModified" (case-insensitive)

        // Distance range from UI (km). Backend sẽ convert sang mét.
        public double? DistanceMin { get; set; }  // km
        public double? DistanceMax { get; set; }  // km

        // Total price range
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
    }

    public enum SortBy { LastModified, CreatedAt, TotalPrice, Id }
    public enum SortDir { Desc, Asc }

    public record SortOption(SortBy By, SortDir Dir);

    public record IncludeOption(
        bool Technician,
        bool Items,
        bool WorkOrdersWithTech
    );
}
