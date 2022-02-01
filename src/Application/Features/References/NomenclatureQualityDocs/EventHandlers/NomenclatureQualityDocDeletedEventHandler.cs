using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.EventHandlers
{
    public class NomenclatureQualityDocDeletedEventHandler : INotificationHandler<DomainEventNotification<NomenclatureQualityDocDeletedEvent>>
    {
        private readonly ILogger<NomenclatureQualityDocDeletedEventHandler> _logger;

        public NomenclatureQualityDocDeletedEventHandler(
            ILogger<NomenclatureQualityDocDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<NomenclatureQualityDocDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}