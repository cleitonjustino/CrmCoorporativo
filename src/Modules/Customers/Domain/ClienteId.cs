namespace Modules.Customers.Domain;

public readonly record struct ClienteId(Guid Value)
{
    public static ClienteId Novo() => new(Guid.NewGuid());
}
