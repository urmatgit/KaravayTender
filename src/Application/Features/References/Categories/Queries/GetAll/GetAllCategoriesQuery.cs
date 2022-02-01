// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {

        public int DirectionId { get; set; }
    }

    public class GetAllCategoriesQueryHandler :
         IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllCategoriesQueryHandler> _localizer;

        public GetAllCategoriesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllCategoriesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllCategoriesQueryHandler method
            Expression<Func<Category, bool>> filters = x => true;// PredicateBuilder.FromFilter<Category>("");
            if (request.DirectionId > 0)
            {

                filters = filters.And(p => p.DirectionId == request.DirectionId);

            }
            var data = await _context.Categories.Where(filters)
                         .Include(c => c.Direction)
                         .OrderBy(c=>c.Direction.Name)
                         .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

