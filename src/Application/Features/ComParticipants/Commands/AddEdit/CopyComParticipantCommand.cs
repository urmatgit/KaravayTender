using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.AddEdit
{
    public class CopyComParticipantCommand: IRequest<Result<int>>, IMapFrom<ComParticipant>
    {
        public int ComOfferId { get; set; }
        public int NewComOfferId { get; set; }
        public string CacheKey => ComParticipantCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComParticipantCacheTokenSource.ResetCacheToken;
    }

    public class CopyComParticipantCommandHandler : IRequestHandler<CopyComParticipantCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CopyComParticipantCommandHandler> _localizer;
        public CopyComParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CopyComParticipantCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CopyComParticipantCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CopyComParticipantCommandHandler method 

            var items = await _context.ComParticipants.AsNoTracking()
               .Where(c => c.ComOfferId == request.ComOfferId)
               .ToListAsync(cancellationToken);
            if (items!=null && items.Count > 0)
            {
                List<ComParticipant> ComParticipants=new List<ComParticipant>();
                foreach(var item in items)
                {
                    item.ComOfferId = request.NewComOfferId;
                    
                    item.LastModified = null;
                    item.LastModifiedBy = "";
                    item.CreatedBy = "";
                }
                await _context.ComParticipants.AddRangeAsync(items, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(items.Count);
            }



           return Result<int>.Success(0);
        }
         
    }
}
