// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Razor.Application.Features.Contragents.EventHandlers
{
    public class ContragentUpdatedEventHandler : INotificationHandler<DomainEventNotification<ContragentUpdatedEvent>>
    {
        private readonly ILogger<ContragentUpdatedEventHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        public ContragentUpdatedEventHandler(
            ILogger<ContragentUpdatedEventHandler> logger,
            IApplicationDbContext context,
             ICurrentUserService currentUserService,
             IDateTime dateTime
            )
        {
            _dateTime = dateTime;
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }
        public async Task Handle(DomainEventNotification<ContragentUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);
            var lastStatusLog = await _context
                .StatusLogs
                .Where(s => s.ContragentId == domainEvent.Item.Id)
                .OrderBy(s => s.Id)
                .LastOrDefaultAsync(cancellationToken);
            if (lastStatusLog is null || !domainEvent.Item.Status.Equals(lastStatusLog.Status))
            {

                var statusLog = new StatusLog
                {
                    ManagerId = _currentUserService.UserId,
                    ContragentId = domainEvent.Item.Id,
                    Status = domainEvent.Item.Status,
                    Description = domainEvent.Description,
                    DateTime = domainEvent.Item.LastModified ?? _dateTime.Now
                };
                _context.StatusLogs.Add(statusLog);
                var createevent = new StatusLogEvent(statusLog);
                statusLog.DomainEvents.Add(createevent);
                await _context.SaveChangesAsync(cancellationToken);
            }


            //return Task.CompletedTask;
        }
    }
}
