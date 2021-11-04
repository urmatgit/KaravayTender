using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class StatusLogDeletedEvent : DomainEvent
    {
        public StatusLogDeletedEvent(StatusLog item)
        {
            Item = item;
        }

        public StatusLog Item { get; }
    }
}