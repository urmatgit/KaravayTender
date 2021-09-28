using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class DirectionUpdatedEvent : DomainEvent
    {
        public DirectionUpdatedEvent(Direction item)
        {
            Item = item;
        }

        public Direction Item { get; }
    }
}
