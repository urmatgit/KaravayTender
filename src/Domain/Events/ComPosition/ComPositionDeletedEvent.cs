using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComPositionDeletedEvent : DomainEvent
    {
        public ComPositionDeletedEvent(ComPosition item)
        {
            Item = item;
        }

        public ComPosition Item { get; }
    }
}
