// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Products.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Products.Queries.GetAll
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {

    }

    public class GetAllProductsQueryHandler :
         IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllProductsQueryHandler> _localizer;

        public GetAllProductsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllProductsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllProductsQueryHandler method 
            throw new NotImplementedException();
        }
    }
}

