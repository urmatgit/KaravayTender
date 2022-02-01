using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class StageParticipantDeletedEvent : DomainEvent
    {
        public StageParticipantDeletedEvent(StageParticipant item)
        {
            Item = item;
        }

        public StageParticipant Item { get; }
    }
}
