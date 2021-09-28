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
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetAll
{
    public class GetAllContragentsQuery : IRequest<IEnumerable<ContragentDto>>
    {
       
    }
    
    public class GetAllContragentsQueryHandler :
         IRequestHandler<GetAllContragentsQuery, IEnumerable<ContragentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllContragentsQueryHandler> _localizer;

        public GetAllContragentsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllContragentsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ContragentDto>> Handle(GetAllContragentsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllContragentsQueryHandler method 
            var data = await _context.Contragents
                         .ProjectTo<ContragentDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

