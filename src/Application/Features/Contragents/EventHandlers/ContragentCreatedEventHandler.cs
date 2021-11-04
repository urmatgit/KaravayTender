using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.Contragents.EventHandlers
{
    public class ContragentCreatedEventHandler : INotificationHandler<DomainEventNotification<ContragentCreatedEvent>>
    {
        private readonly ILogger<ContragentCreatedEventHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public ContragentCreatedEventHandler(
            ILogger<ContragentCreatedEventHandler> logger,
               IApplicationDbContext context,
               ICurrentUserService currentUserService
            )
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }
        public async Task Handle(DomainEventNotification<ContragentCreatedEvent> notification, CancellationToken cancellationToken)
        {
            
            var domainEvent = notification.DomainEvent;
            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);
            var statusLog = new StatusLog
            {
                ManagerId =_currentUserService.UserId,
                ContragentId = domainEvent.Item.Id,
                Status=domainEvent.Item.Status,
                DateTime=domainEvent.Item.Created
            };
            _context.StatusLogs.Add(statusLog);
            var createevent = new StatusLogCreatedEvent(statusLog);
            statusLog.DomainEvents.Add(createevent);
            await _context.SaveChangesAsync(cancellationToken);
            

            //return Task.CompletedTask;
        }
    }
}
