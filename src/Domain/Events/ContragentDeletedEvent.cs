using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ContragentDeletedEvent : DomainEvent
    {
        public ContragentDeletedEvent(Contragent item)
        {
            Item = item;
        }

        public Contragent Item { get; }
    }
}
