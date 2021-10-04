using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Contragents.Caching;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit
{
    public class AddEditContragentCommand: ContragentDto,IRequest<Result<int>>, IMapFrom<Contragent>
    {
        public string CacheKey => ContragentCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ContragentCacheTokenSource.ResetCacheToken;
        
    }

    public class AddEditContragentCommandHandler : IRequestHandler<AddEditContragentCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditContragentCommandHandler> _localizer;
        public AddEditContragentCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditContragentCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            
        }
        //private async Task LoadDirection()
        //{
        //    var data = await _context.Directions
        //               .Include(d => d.Categories)
        //               .ProjectTo<DirectionDto>(_mapper.ConfigurationProvider)
        //               .ToListAsync();

        //}
        public async Task<Result<int>> Handle(AddEditContragentCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditContragentCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.Contragents.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<Contragent>(request);
                _context.Contragents.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
