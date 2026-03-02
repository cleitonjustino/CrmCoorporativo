using BuildingBlocks.Domain;

namespace Modules.Customers.Domain;

public sealed record ClienteCriado(
    ClienteId ClienteId,
    Documento Documento,
    Email Email,
    Endereco? Endereco,
    InscricaoEstadual? InscricaoEstadual,
    DataNascimento? DataNascimento,
    DataFundacaoEmpresa? DataFundacaoEmpresa,
    MetadadosEvento Metadados,
    DateTime OccurredOnUtc) : IDomainEvent;

public sealed record EmailAlterado(
    ClienteId ClienteId,
    Email EmailAntigo,
    Email NovoEmail,
    MetadadosEvento Metadados,
    DateTime OccurredOnUtc) : IDomainEvent;

public sealed record EnderecoAtualizado(
    ClienteId ClienteId,
    Endereco Endereco,
    MetadadosEvento Metadados,
    DateTime OccurredOnUtc) : IDomainEvent;

public sealed record ClienteExcluido(
    ClienteId ClienteId,
    MetadadosEvento Metadados,
    DateTime OccurredOnUtc) : IDomainEvent;
