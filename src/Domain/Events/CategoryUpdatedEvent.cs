using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class CategoryUpdatedEvent : DomainEvent
    {
        public CategoryUpdatedEvent(Category item)
        {
            Item = item;
        }

        public Category Item { get; }
    }
}
