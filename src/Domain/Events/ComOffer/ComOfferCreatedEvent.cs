using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComOfferCreatedEvent : DomainEvent
    {
        public ComOfferCreatedEvent(ComOffer item)
        {
            Item = item;
        }

        public ComOffer Item { get; }
    }
}
