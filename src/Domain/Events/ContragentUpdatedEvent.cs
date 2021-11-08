using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ContragentUpdatedEvent : DomainEvent
    {
        public ContragentUpdatedEvent(Contragent item,string description="")
        {
            Item = item;
            Description = description;
        }

        public Contragent Item { get; }
        public string Description { get; }
    }
}
