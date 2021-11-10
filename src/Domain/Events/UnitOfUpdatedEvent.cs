using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class UnitOfUpdatedEvent : DomainEvent
    {
        public UnitOfUpdatedEvent(UnitOf item)
        {
            Item = item;
        }

        public UnitOf Item { get; }
    }
}
