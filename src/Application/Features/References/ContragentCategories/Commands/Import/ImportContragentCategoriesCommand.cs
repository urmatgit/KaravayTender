// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Import
{
    public class ImportContragentCategoriesCommand : IRequest<Result>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class CreateContragentCategoriesTemplateCommand : IRequest<byte[]>
    {
        public IEnumerable<string> Fields { get; set; }
        public string SheetName { get; set; }
    }

    public class ImportContragentCategoriesCommandHandler :
                 IRequestHandler<CreateContragentCategoriesTemplateCommand, byte[]>,
                 IRequestHandler<ImportContragentCategoriesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportContragentCategoriesCommandHandler> _localizer;
        private readonly IExcelService _excelService;

        public ImportContragentCategoriesCommandHandler(
            IApplicationDbContext context,
            IExcelService excelService,
            IStringLocalizer<ImportContragentCategoriesCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _excelService = excelService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportContragentCategoriesCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportContragentCategoriesCommandHandler method
            var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ContragentCategoryDto, object>>
            {
                //ex. { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },

            }, _localizer["ContragentCategories"]);
            throw new System.NotImplementedException();
        }
        public async Task<byte[]> Handle(CreateContragentCategoriesTemplateCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportContragentCategoriesCommandHandler method 
            var fields = new string[] {
                   //TODO:Defines the title and order of the fields to be imported's template
                   //_localizer["Name"],
                };
            var result = await _excelService.CreateTemplateAsync(fields, _localizer["ContragentCategories"]);
            return result;
        }
    }
}
