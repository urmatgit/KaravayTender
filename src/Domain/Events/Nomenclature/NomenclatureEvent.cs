using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class NomenclatureEvent : DomainEvent
    {
        public NomenclatureEvent(Nomenclature item)
        {
            Item = item;
        }

        public Nomenclature Item { get; }
    }
}
