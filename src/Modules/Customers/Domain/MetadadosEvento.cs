namespace Modules.Customers.Domain;

public sealed record MetadadosEvento(
    string? UsuarioId,
    string? UsuarioEmail,
    IReadOnlyCollection<string> Papeis,
    string? CorrelationId,
    DateTime TimestampUtc);
