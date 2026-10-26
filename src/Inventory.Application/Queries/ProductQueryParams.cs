using System;

namespace Inventory.Application.Queries;

public sealed record ProductQueryParams(
int? Id,
string? Name,
int Page = 1,
int PageSize = 10);
