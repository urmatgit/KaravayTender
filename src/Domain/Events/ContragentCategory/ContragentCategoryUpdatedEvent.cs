using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ContragentCategoryUpdatedEvent : DomainEvent
    {
        public ContragentCategoryUpdatedEvent(ContragentCategory item)
        {
            Item = item;
        }

        public ContragentCategory Item { get; }
    }
}
