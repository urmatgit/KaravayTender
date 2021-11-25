using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class ComOfferDeletedEvent : DomainEvent
    {
        public ComOfferDeletedEvent(ComOffer item)
        {
            Item = item;
        }

        public ComOffer Item { get; }
    }
}
