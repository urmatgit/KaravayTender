using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComPositionCreatedEvent : DomainEvent
    {
        public ComPositionCreatedEvent(ComPosition item)
        {
            Item = item;
        }

        public ComPosition Item { get; }
    }
}
