using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComStageUpdatedEvent : DomainEvent
    {
        public ComStageUpdatedEvent(ComStage item)
        {
            Item = item;
        }

        public ComStage Item { get; }
    }
}
