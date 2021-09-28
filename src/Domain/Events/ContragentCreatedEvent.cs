using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ContragentCreatedEvent : DomainEvent
    {
        public ContragentCreatedEvent(Contragent item)
        {
            Item = item;
        }

        public Contragent Item { get; }
    }
}
