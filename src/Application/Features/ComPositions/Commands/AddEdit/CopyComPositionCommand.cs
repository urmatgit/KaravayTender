using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComPositions.Caching;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Commands.AddEdit
{
    public class CopyComPositionsCommand: IRequest<Result<int>>, IMapFrom<ComPosition>
    {
        public int ComOfferId { get; set; }
        public int NewComOfferId { get; set; }
        public string CacheKey => ComPositionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComPositionCacheTokenSource.ResetCacheToken;
    }

    public class CopyComPositionsCommandHandler : IRequestHandler<CopyComPositionsCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CopyComPositionsCommandHandler> _localizer;
        private readonly IDateTime _dateTime;
        public CopyComPositionsCommandHandler(
            IApplicationDbContext context,
            IDateTime dateTime,
            IStringLocalizer<CopyComPositionsCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _dateTime = dateTime;
        }
        public async Task<Result<int>> Handle(CopyComPositionsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CopyComPositionsCommandHandler method 

            var items = await _context.ComPositions.AsNoTracking()
                .Include(a => a.AreaComPositions)
               .Where(c => c.ComOfferId == request.ComOfferId)
               .ToListAsync(cancellationToken);
            if (items!=null && items.Count > 0)
            {
                List<ComPosition> comPositions=new List<ComPosition>();
                foreach(var item in items)
                {
                    item.Id = 0;
                    item.StageCompositions = null;
                    item.ComOfferId = request.NewComOfferId;
                    item.Created = _dateTime.Now;
                    item.LastModified = null;
                    item.LastModifiedBy = "";
                    item.CreatedBy = "";
                }
                await _context.ComPositions.AddRangeAsync(items, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(items.Count);
            }



           return Result<int>.Success(0);
        }
      
    }
}
