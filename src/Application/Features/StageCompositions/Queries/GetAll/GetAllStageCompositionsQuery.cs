﻿using System;
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
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Queries.GetAll
{
    public class GetAllStageCompositionsQuery : IRequest<IEnumerable<StageCompositionDto>>
    {
       
    }
    
    public class GetAllStageCompositionsQueryHandler :
         IRequestHandler<GetAllStageCompositionsQuery, IEnumerable<StageCompositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllStageCompositionsQueryHandler> _localizer;

        public GetAllStageCompositionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllStageCompositionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<StageCompositionDto>> Handle(GetAllStageCompositionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllStageCompositionsQueryHandler method 
            var data = await _context.StageCompositions
                         .ProjectTo<StageCompositionDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

