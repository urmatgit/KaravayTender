using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComStageCreatedEvent : DomainEvent
    {
        public ComStageCreatedEvent(ComStage item)
        {
            Item = item;
        }

        public ComStage Item { get; }
    }
}
