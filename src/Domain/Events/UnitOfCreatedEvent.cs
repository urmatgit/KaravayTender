using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class UnitOfCreatedEvent : DomainEvent
    {
        public UnitOfCreatedEvent(UnitOf item)
        {
            Item = item;
        }

        public UnitOf Item { get; }
    }
}
