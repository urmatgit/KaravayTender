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
using CleanArchitecture.Razor.Application.Features.ApprovalDatas.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ApprovalDatas.Queries.GetAll
{
    public class GetAllApprovalDatasQuery : IRequest<IEnumerable<ApprovalDataDto>>
    {
       
    }
    
    public class GetAllApprovalDatasQueryHandler :
         IRequestHandler<GetAllApprovalDatasQuery, IEnumerable<ApprovalDataDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllApprovalDatasQueryHandler> _localizer;

        public GetAllApprovalDatasQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllApprovalDatasQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public Task<IEnumerable<ApprovalDataDto>> Handle(GetAllApprovalDatasQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllApprovalDatasQueryHandler method 
            throw new NotImplementedException();
        }
    }
}

