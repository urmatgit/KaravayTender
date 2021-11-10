using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ProductDeletedEvent : DomainEvent
    {
        public ProductDeletedEvent(Product item)
        {
            Item = item;
        }

        public Product Item { get; }
    }
}
