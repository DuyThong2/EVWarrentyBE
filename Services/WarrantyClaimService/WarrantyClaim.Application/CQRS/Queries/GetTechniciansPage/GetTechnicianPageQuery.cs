using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Queries.GetTechniciansPage
{
    public record GetTechniciansFilteredQuery(
        TechniciansFilter Filter,
        PaginationRequest Pagination,
        SortOption Sort
    ) : IQuery<GetTechniciansFilteredResult>;

    public record GetTechniciansFilteredResult(PaginatedResult<TechnicianDto> Result);

    public class TechniciansFilter
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
    }

    public enum TechSortBy { FullName, CreatedAt, LastModified, Id }
    public enum TechSortDir { Desc, Asc }

    public record SortOption(TechSortBy By, TechSortDir Dir);
}
