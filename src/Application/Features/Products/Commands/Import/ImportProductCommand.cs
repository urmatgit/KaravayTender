// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Products.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Products.Commands.Import
{
    public class ImportProductCommand : ProductDto, IRequest<Result>, IMapFrom<Product>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class CreateProductTemplateCommand : IRequest<byte[]>
    {
        public IEnumerable<string> Fields { get; set; }
        public string SheetName { get; set; }
    }

    public class ImportProductCommandHandler : IRequestHandler<ImportProductCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportProductCommandHandler> _localizer;
        public ImportProductCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<ImportProductCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportProductCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportProductCommandHandler method 
            throw new System.NotImplementedException();
        }
    }
}
