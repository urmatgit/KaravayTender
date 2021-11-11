using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class NomenclatureQualityDocDeletedEvent : DomainEvent
    {
        public NomenclatureQualityDocDeletedEvent(NomenclatureQualityDoc item)
        {
            Item = item;
        }

        public NomenclatureQualityDoc Item { get; }
    }
}
