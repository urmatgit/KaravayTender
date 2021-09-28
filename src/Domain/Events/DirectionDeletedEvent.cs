using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class DirectionDeletedEvent : DomainEvent
    {
        public DirectionDeletedEvent(Direction item)
        {
            Item = item;
        }

        public Direction Item { get; }
    }
}
