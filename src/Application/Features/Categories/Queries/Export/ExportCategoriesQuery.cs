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
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Categories.Queries.Export
{
    public class ExportCategoriesQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportCategoriesQueryHandler :
         IRequestHandler<ExportCategoriesQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportCategoriesQueryHandler> _localizer;

        public ExportCategoriesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportCategoriesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportCategoriesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportCategoriesQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Category>(request.FilterRules);
            var data = await _context.Categories.Where(filters)
                        .Include(c=>c.Direction)
                       .OrderBy($"{request.Sort} {request.Order}") 
                       .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<CategoryDto, object>>()
                {
                    { _localizer["Id"], item => item.Id },
                    { _localizer["Name"], item => item.Name },
                    { _localizer["Direction"], item => item.Direction.Name },
                    { _localizer["Description"], item => item.Description }

                }
                , _localizer["Categories"]);
            return result;
        }
    }
}

