// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Queries.GetAll
{
    public class GetByContragentCategoryQuery : IRequest<IEnumerable<ContragentCategoryDto>>
    {
        public int ContragentId { get; set; }

    }

    public class GetByContragentCategoryQueryHandler :
         IRequestHandler<GetByContragentCategoryQuery, IEnumerable<ContragentCategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetByContragentCategoryQueryHandler> _localizer;

        public GetByContragentCategoryQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetByContragentCategoryQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ContragentCategoryDto>> Handle(GetByContragentCategoryQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllContragentCategoriesQueryHandler method 
            var data = await _context.ContragentCategories
                         .Include(c => c.Category)
                         .Where(c => c.ContragentId == request.ContragentId)
                         .ProjectTo<ContragentCategoryDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

