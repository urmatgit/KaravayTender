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
    public class AreaComPositionUpdatedEventHandler : INotificationHandler<DomainEventNotification<AreaComPositionUpdatedEvent>>
    {
        private readonly ILogger<AreaComPositionUpdatedEventHandler> _logger;

        public AreaComPositionUpdatedEventHandler(
            ILogger<AreaComPositionUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<AreaComPositionUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}