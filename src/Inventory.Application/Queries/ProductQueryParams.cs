using Inventory.Application.Common.Enums;

namespace Inventory.Application.Queries;

public sealed record ProductQueryParams(
int? Id,
string? Name,
string Q,
string? SortBy,
SortDirection SortDir = SortDirection.Asc,
int Page = 1,
int PageSize = 10);
