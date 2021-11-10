using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ContragentCategoryDeletedEvent : DomainEvent
    {
        public ContragentCategoryDeletedEvent(ContragentCategory item)
        {
            Item = item;
        }

        public ContragentCategory Item { get; }
    }
}
