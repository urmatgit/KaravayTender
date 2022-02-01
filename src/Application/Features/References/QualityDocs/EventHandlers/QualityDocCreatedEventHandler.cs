using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.EventHandlers
{
    public class QualityDocCreatedEventHandler : INotificationHandler<DomainEventNotification<QualityDocEvent>>
    {
        private readonly ILogger<QualityDocCreatedEventHandler> _logger;

        public QualityDocCreatedEventHandler(
            ILogger<QualityDocCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<QualityDocEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
