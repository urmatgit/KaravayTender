using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.Caching;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.AddEdit
{
    public class AddEditUnitOfCommand: UnitOfDto,IRequest<Result<int>>, IMapFrom<UnitOf>
    {
        public string CacheKey => UnitOfCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => UnitOfCacheTokenSource.ResetCacheToken;
    }

    public class AddEditUnitOfCommandHandler : IRequestHandler<AddEditUnitOfCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditUnitOfCommandHandler> _localizer;
        public AddEditUnitOfCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditUnitOfCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditUnitOfCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditUnitOfCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.UnitOfs.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<UnitOf>(request);
                _context.UnitOfs.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
