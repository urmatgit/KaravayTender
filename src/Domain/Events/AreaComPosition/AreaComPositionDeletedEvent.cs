using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class AreaComPositionDeletedEvent : DomainEvent
    {
        public AreaComPositionDeletedEvent(AreaComPosition item)
        {
            Item = item;
        }

        public AreaComPosition Item { get; }
    }
}
