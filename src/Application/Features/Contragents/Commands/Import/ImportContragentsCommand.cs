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
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Import
{
    public class ImportContragentsCommand : IRequest<Result>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class CreateContragentsTemplateCommand : IRequest<byte[]>
    {
        public IEnumerable<string> Fields { get; set; }
        public string SheetName { get; set; }
    }

    public class ImportContragentsCommandHandler :
                 IRequestHandler<CreateContragentsTemplateCommand, byte[]>,
                 IRequestHandler<ImportContragentsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportContragentsCommandHandler> _localizer;
        private readonly IExcelService _excelService;

        public ImportContragentsCommandHandler(
            IApplicationDbContext context,
            IExcelService excelService,
            IStringLocalizer<ImportContragentsCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _excelService = excelService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportContragentsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportContragentsCommandHandler method
            var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ContragentDto, object>>
            {
                //ex. { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },

            }, _localizer["Contragents"]);
            throw new System.NotImplementedException();
        }
        public async Task<byte[]> Handle(CreateContragentsTemplateCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportContragentsCommandHandler method 
            var fields = new string[] {
                   //TODO:Defines the title and order of the fields to be imported's template
                   //_localizer["Name"],
                };
            var result = await _excelService.CreateTemplateAsync(fields, _localizer["Contragents"]);
            return result;
        }
    }
}
