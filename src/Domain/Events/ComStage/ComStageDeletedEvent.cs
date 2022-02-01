using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComStageDeletedEvent : DomainEvent
    {
        public ComStageDeletedEvent(ComStage item)
        {
            Item = item;
        }

        public ComStage Item { get; }
    }
}
