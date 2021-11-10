using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ProductUpdatedEvent : DomainEvent
    {
        public ProductUpdatedEvent(Product item)
        {
            Item = item;
        }

        public Product Item { get; }
    }
}
