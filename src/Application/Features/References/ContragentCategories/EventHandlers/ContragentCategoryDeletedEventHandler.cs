using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.EventHandlers
{
    public class ContragentCategoryDeletedEventHandler : INotificationHandler<DomainEventNotification<ContragentCategoryDeletedEvent>>
    {
        private readonly ILogger<ContragentCategoryDeletedEventHandler> _logger;

        public ContragentCategoryDeletedEventHandler(
            ILogger<ContragentCategoryDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<ContragentCategoryDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}