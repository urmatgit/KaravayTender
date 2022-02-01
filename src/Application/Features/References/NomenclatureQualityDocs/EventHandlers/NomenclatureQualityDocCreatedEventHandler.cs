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
    public class NomenclatureQualityDocCreatedEventHandler : INotificationHandler<DomainEventNotification<NomenclatureQualityDocCreatedEvent>>
    {
        private readonly ILogger<NomenclatureQualityDocCreatedEventHandler> _logger;

        public NomenclatureQualityDocCreatedEventHandler(
            ILogger<NomenclatureQualityDocCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<NomenclatureQualityDocCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}