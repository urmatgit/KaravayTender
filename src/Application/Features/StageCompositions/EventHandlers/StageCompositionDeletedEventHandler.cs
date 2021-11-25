﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.EventHandlers
{
    public class StageCompositionDeletedEventHandler : INotificationHandler<DomainEventNotification<StageCompositionDeletedEvent>>
    {
        private readonly ILogger<StageCompositionDeletedEventHandler> _logger;

        public StageCompositionDeletedEventHandler(
            ILogger<StageCompositionDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<StageCompositionDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}