using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class AreaComPositionUpdatedEvent : DomainEvent
    {
        public AreaComPositionUpdatedEvent(AreaComPosition item)
        {
            Item = item;
        }

        public AreaComPosition Item { get; }
    }
}
