using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Domain.Entities;
using System.Linq.Dynamic.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Localization;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetAll
{
    public class GetAllComStagesQuery : IRequest<IEnumerable<ComStageDto>>
    {
       
    }
    
    public class GetAllComStagesQueryHandler :
         IRequestHandler<GetAllComStagesQuery, IEnumerable<ComStageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllComStagesQueryHandler> _localizer;

        public GetAllComStagesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllComStagesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ComStageDto>> Handle(GetAllComStagesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllComStagesQueryHandler method 
            var data = await _context.ComStages
                         .ProjectTo<ComStageDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

