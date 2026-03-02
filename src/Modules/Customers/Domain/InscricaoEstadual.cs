using BuildingBlocks.SharedKernel;

namespace Modules.Customers.Domain;

public sealed record InscricaoEstadual(string? Numero, bool Isento)
{
    public static Result<InscricaoEstadual> Criar(string? numero, bool isento)
    {
        if (isento) return Result<InscricaoEstadual>.Success(new InscricaoEstadual(null, true));

        if (string.IsNullOrWhiteSpace(numero))
        {
            return Result<InscricaoEstadual>.Failure(new Error("inscricao_estadual.obrigatoria", "IE é obrigatória ou deve ser marcado como Isento."));
        }

        return Result<InscricaoEstadual>.Success(new InscricaoEstadual(numero.Trim(), false));
    }
}
