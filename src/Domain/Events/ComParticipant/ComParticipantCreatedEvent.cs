using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComParticipantCreatedEvent : DomainEvent
    {
        public ComParticipantCreatedEvent(ComParticipant item)
        {
            Item = item;
        }

        public ComParticipant Item { get; }
    }
}
