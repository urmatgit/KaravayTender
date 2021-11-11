// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.EventHandlers
{
    public class ContragentCategoryCreatedEventHandler : INotificationHandler<DomainEventNotification<ContragentCategoryCreatedEvent>>
    {
        private readonly ILogger<ContragentCategoryCreatedEventHandler> _logger;

        public ContragentCategoryCreatedEventHandler(
            ILogger<ContragentCategoryCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<ContragentCategoryCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}