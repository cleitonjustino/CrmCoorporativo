using BuildingBlocks.SharedKernel;

namespace Modules.Customers.Domain;

public sealed record Endereco(
    string Cep,
    string Logradouro,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string Uf)
{
    public static Result<Endereco> Criar(
        string cep,
        string logradouro,
        string numero,
        string? complemento,
        string bairro,
        string cidade,
        string uf)
    {
        cep = Guard.AgainstNullOrWhiteSpace(cep, nameof(cep)).Trim();
        logradouro = Guard.AgainstNullOrWhiteSpace(logradouro, nameof(logradouro)).Trim();
        numero = Guard.AgainstNullOrWhiteSpace(numero, nameof(numero)).Trim();
        bairro = Guard.AgainstNullOrWhiteSpace(bairro, nameof(bairro)).Trim();
        cidade = Guard.AgainstNullOrWhiteSpace(cidade, nameof(cidade)).Trim();
        uf = Guard.AgainstNullOrWhiteSpace(uf, nameof(uf)).Trim();

        if (uf.Length != 2)
        {
            return Result<Endereco>.Failure(new Error("endereco.uf_invalida", "UF deve ter 2 caracteres."));
        }

        return Result<Endereco>.Success(new Endereco(cep, logradouro, numero, complemento?.Trim(), bairro, cidade, uf.ToUpperInvariant()));
    }
}
