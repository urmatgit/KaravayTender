using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.EventHandlers
{
    public class StatusLogDeletedEventHandler : INotificationHandler<DomainEventNotification<StatusLogEvent>>
    {
        private readonly ILogger<StatusLogDeletedEventHandler> _logger;

        public StatusLogDeletedEventHandler(
            ILogger<StatusLogDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<StatusLogEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}