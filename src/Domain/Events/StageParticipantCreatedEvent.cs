using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class StageParticipantCreatedEvent : DomainEvent
    {
        public StageParticipantCreatedEvent(StageParticipant item)
        {
            Item = item;
        }

        public StageParticipant Item { get; }
    }
}
