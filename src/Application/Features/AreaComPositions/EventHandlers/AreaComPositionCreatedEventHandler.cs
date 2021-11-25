using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.EventHandlers
{
    public class AreaComPositionCreatedEventHandler : INotificationHandler<DomainEventNotification<AreaComPositionCreatedEvent>>
    {
        private readonly ILogger<AreaComPositionCreatedEventHandler> _logger;

        public AreaComPositionCreatedEventHandler(
            ILogger<AreaComPositionCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<AreaComPositionCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}