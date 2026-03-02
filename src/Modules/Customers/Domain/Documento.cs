using System.Text.RegularExpressions;
using BuildingBlocks.SharedKernel;

namespace Modules.Customers.Domain;

public enum TipoDocumento
{
    Cpf = 1,
    Cnpj = 2
}

public sealed record Documento
{
    private static readonly Regex DigitosApenas = new("\\D+", RegexOptions.Compiled);

    private Documento(string valor, TipoDocumento tipo)
    {
        Valor = valor;
        Tipo = tipo;
    }

    public string Valor { get; }
    public TipoDocumento Tipo { get; }

    public static Result<Documento> Criar(string bruto)
    {
        bruto = Guard.AgainstNullOrWhiteSpace(bruto, nameof(bruto));
        var normalizado = DigitosApenas.Replace(bruto, string.Empty);

        if (normalizado.Length == 11)
        {
            return CpfValido(normalizado)
                ? Result<Documento>.Success(new Documento(normalizado, TipoDocumento.Cpf))
                : Result<Documento>.Failure(new Error("documento.cpf_invalido", "CPF inválido."));
        }

        if (normalizado.Length == 14)
        {
            return CnpjValido(normalizado)
                ? Result<Documento>.Success(new Documento(normalizado, TipoDocumento.Cnpj))
                : Result<Documento>.Failure(new Error("documento.cnpj_invalido", "CNPJ inválido."));
        }

        return Result<Documento>.Failure(new Error("documento.tamanho_invalido", "Documento deve ser CPF (11) ou CNPJ (14)."));
    }

    private static bool CpfValido(string cpf)
    {
        if (cpf.Distinct().Count() == 1) return false;

        var digits = cpf.Select(c => c - '0').ToArray();

        int sum1 = 0;
        for (int i = 0, weight = 10; i < 9; i++, weight--) sum1 += digits[i] * weight;
        var d1 = (sum1 * 10) % 11;
        if (d1 == 10) d1 = 0;
        if (digits[9] != d1) return false;

        int sum2 = 0;
        for (int i = 0, weight = 11; i < 10; i++, weight--) sum2 += digits[i] * weight;
        var d2 = (sum2 * 10) % 11;
        if (d2 == 10) d2 = 0;

        return digits[10] == d2;
    }

    private static bool CnpjValido(string cnpj)
    {
        if (cnpj.Distinct().Count() == 1) return false;

        var digits = cnpj.Select(c => c - '0').ToArray();
        int[] w1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] w2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int sum1 = 0;
        for (int i = 0; i < 12; i++) sum1 += digits[i] * w1[i];
        int r1 = sum1 % 11;
        int d1 = r1 < 2 ? 0 : 11 - r1;
        if (digits[12] != d1) return false;

        int sum2 = 0;
        for (int i = 0; i < 13; i++) sum2 += digits[i] * w2[i];
        int r2 = sum2 % 11;
        int d2 = r2 < 2 ? 0 : 11 - r2;

        return digits[13] == d2;
    }
}
