namespace BuildingBlocks.Domain;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}

public abstract class AggregateRoot<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> _uncommittedEvents = new();

    public TId Id { get; protected set; } = default!;
    public long Version { get; protected set; } = 0;

    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => _uncommittedEvents.AsReadOnly();

    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    protected void Raise(IDomainEvent @event)
    {
        Apply(@event);
        _uncommittedEvents.Add(@event);
    }

    public void LoadFromHistory(IEnumerable<IDomainEvent> history, long version)
    {
        foreach (var e in history)
        {
            Apply(e);
        }

        Version = version;
        ClearUncommittedEvents();
    }

    protected abstract void Apply(IDomainEvent @event);
}
