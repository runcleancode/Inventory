namespace Inventory.Application.Common.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string resourceName, object id) : base($"{resourceName} with id '{id}' was not found.", statusCode: 404) { }
}
