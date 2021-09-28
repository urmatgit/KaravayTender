using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ContragentUpdatedEvent : DomainEvent
    {
        public ContragentUpdatedEvent(Contragent item)
        {
            Item = item;
        }

        public Contragent Item { get; }
    }
}
