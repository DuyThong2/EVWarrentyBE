using BuildingBlocks.Pagination;
using System;

namespace Vehicle.Application.CQRS.Customers.Queries.GetCustomersByFilter
{
    public record GetCustomersByFilterQuery(
        CustomersFilter Filter,
        PaginationRequest Pagination,
        SortOption Sort
    ) : IQuery<GetCustomersByFilterResult>;

    public record CustomersFilter
    {
        public Guid? CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
    }

    public enum SortBy { FullName, Email, PhoneNumber, Address, CreatedAt, CustomerId }
    public enum SortDir { Asc, Desc }

    public record SortOption(SortBy By, SortDir Dir);
}
