using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class StatusLogUpdatedEvent : DomainEvent
    {
        public StatusLogUpdatedEvent(StatusLog item)
        {
            Item = item;
        }

        public StatusLog Item { get; }
    }
}
