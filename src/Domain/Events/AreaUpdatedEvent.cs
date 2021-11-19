using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class AreaUpdatedEvent : DomainEvent
    {
        public AreaUpdatedEvent(Area item)
        {
            Item = item;
        }

        public Area Item { get; }
    }
}
