namespace Modules.Customers.Domain;

public readonly record struct DataNascimento(DateOnly Value)
{
    public int ObterIdadeEm(DateOnly em)
    {
        var idade = em.Year - Value.Year;
        if (em < Value.AddYears(idade)) idade--;
        return idade;
    }
}

public readonly record struct DataFundacaoEmpresa(DateOnly Value);
