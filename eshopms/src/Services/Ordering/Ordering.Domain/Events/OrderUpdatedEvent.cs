namespace Ordering.Domain.Events
{
    public record OrderUpdatedEvent(Order ordrr) : IDomainEvent;
}
