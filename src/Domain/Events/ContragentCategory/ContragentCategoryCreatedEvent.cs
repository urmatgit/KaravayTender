using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ContragentCategoryCreatedEvent : DomainEvent
    {
        public ContragentCategoryCreatedEvent(ContragentCategory item)
        {
            Item = item;
        }

        public ContragentCategory Item { get; }
    }
}
