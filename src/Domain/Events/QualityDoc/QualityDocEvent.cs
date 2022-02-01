using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class QualityDocEvent : DomainEvent
    {
        public QualityDocEvent(QualityDoc item)
        {
            Item = item;
        }

        public QualityDoc Item { get; }
    }
}
