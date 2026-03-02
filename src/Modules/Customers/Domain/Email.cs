using BuildingBlocks.SharedKernel;

namespace Modules.Customers.Domain;

public sealed record Email
{
    private Email(string value) => Value = value;

    public string Value { get; }

    public static Result<Email> Criar(string bruto)
    {
        bruto = Guard.AgainstNullOrWhiteSpace(bruto, nameof(bruto)).Trim().ToLowerInvariant();

        if (!bruto.Contains('@', StringComparison.Ordinal) || bruto.Length > 320)
        {
            return Result<Email>.Failure(new Error("email.invalido", "Email inválido."));
        }

        return Result<Email>.Success(new Email(bruto));
    }
}
