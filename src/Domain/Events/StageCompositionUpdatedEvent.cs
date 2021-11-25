using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class StageCompositionUpdatedEvent : DomainEvent
    {
        public StageCompositionUpdatedEvent(StageComposition item)
        {
            Item = item;
        }

        public StageComposition Item { get; }
    }
}
