namespace Inventory.Application.Common.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} with key '{key}' was not found") { }

    public override int StatusCode => 404;
}
