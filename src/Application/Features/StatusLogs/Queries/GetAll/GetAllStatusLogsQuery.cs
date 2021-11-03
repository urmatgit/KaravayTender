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
using CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Queries.GetAll
{
    public class GetAllStatusLogsQuery : IRequest<IEnumerable<StatusLogDto>>
    {
       
    }
    
    public class GetAllStatusLogsQueryHandler :
         IRequestHandler<GetAllStatusLogsQuery, IEnumerable<StatusLogDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllStatusLogsQueryHandler> _localizer;

        public GetAllStatusLogsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllStatusLogsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public Task<IEnumerable<StatusLogDto>> Handle(GetAllStatusLogsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllStatusLogsQueryHandler method 
            throw new NotImplementedException();
        }
    }
}

