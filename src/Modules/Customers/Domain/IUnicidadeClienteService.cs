namespace Modules.Customers.Domain;

public interface IUnicidadeClienteService
{
    bool DocumentoDisponivel(Documento documento);
    bool EmailDisponivel(Email email);
    bool EmailDisponivelParaAlteracao(ClienteId clienteId, Email emailAtual, Email novoEmail);
}
