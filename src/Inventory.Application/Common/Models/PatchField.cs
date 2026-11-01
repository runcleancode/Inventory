namespace Inventory.Application.Common.Models;

public readonly struct PatchField<T>
{

    public bool IsSet { get; }
    public T? Value { get; }

    private PatchField(bool isSet, T? value)
    {
        IsSet = isSet;
        Value = value;
    }

    public static PatchField<T> Unset = new(false, default);
    public static PatchField<T> Set(T? value) => new(true, value);
}
