using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class NomenclatureQualityDocUpdatedEvent : DomainEvent
    {
        public NomenclatureQualityDocUpdatedEvent(NomenclatureQualityDoc item)
        {
            Item = item;
        }

        public NomenclatureQualityDoc Item { get; }
    }
}
