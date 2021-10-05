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
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Queries.GetAll
{
    public class GetAllContragentCategoriesQuery : IRequest<IEnumerable<ContragentCategoryDto>>
    {
       
    }
    
    public class GetAllContragentCategoriesQueryHandler :
         IRequestHandler<GetAllContragentCategoriesQuery, IEnumerable<ContragentCategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllContragentCategoriesQueryHandler> _localizer;

        public GetAllContragentCategoriesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllContragentCategoriesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ContragentCategoryDto>> Handle(GetAllContragentCategoriesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllContragentCategoriesQueryHandler method 
            var data = await _context.ContragentCategories
                         .ProjectTo<ContragentCategoryDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

