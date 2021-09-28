using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class CategoryCreatedEvent : DomainEvent
    {
        public CategoryCreatedEvent(Category item)
        {
            Item = item;
        }

        public Category Item { get; }
    }
}
