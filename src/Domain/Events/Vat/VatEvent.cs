using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class VatEvent : DomainEvent
    {
        public VatEvent(Vat item)
        {
            Item = item;
        }

        public Vat Item { get; }
    }
}
