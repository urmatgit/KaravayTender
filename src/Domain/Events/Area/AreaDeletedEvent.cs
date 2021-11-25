using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class AreaDeletedEvent : DomainEvent
    {
        public AreaDeletedEvent(Area item)
        {
            Item = item;
        }

        public Area Item { get; }
    }
}
