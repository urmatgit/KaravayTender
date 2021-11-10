using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Commands.AddEdit
{
    public class AddEditStatusLogCommand: StatusLogDto,IRequest<Result>, IMapFrom<StatusLog>
    {
      
    }

    public class AddEditStatusLogCommandHandler : IRequestHandler<AddEditStatusLogCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditStatusLogCommandHandler> _localizer;
        public AddEditStatusLogCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditStatusLogCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(AddEditStatusLogCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditStatusLogCommandHandler method 
            if (request.Id > 0)
            {
                var customer = await _context.StatusLogs.FindAsync(new object[] { request.Id }, cancellationToken);
                customer = _mapper.Map(request, customer);
                //     _context.Customers.Update(customer);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(customer.Id);
            }
            else
            {
                var customer = _mapper.Map<StatusLog>(request);
                var createevent = new StatusLogEvent(customer);
                customer.DomainEvents.Add(createevent);
                _context.StatusLogs.Add(customer);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(customer.Id);
            }
        }
    }
}
