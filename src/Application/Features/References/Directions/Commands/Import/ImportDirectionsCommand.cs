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
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.Import
{
    public class ImportDirectionsCommand : IRequest<Result>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class CreateDirectionsTemplateCommand : IRequest<byte[]>
    {
        public IEnumerable<string> Fields { get; set; }
        public string SheetName { get; set; }
    }

    public class ImportDirectionsCommandHandler :
                 IRequestHandler<CreateDirectionsTemplateCommand, byte[]>,
                 IRequestHandler<ImportDirectionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportDirectionsCommandHandler> _localizer;
        private readonly IExcelService _excelService;

        public ImportDirectionsCommandHandler(
            IApplicationDbContext context,
            IExcelService excelService,
            IStringLocalizer<ImportDirectionsCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _excelService = excelService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportDirectionsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportDirectionsCommandHandler method
            var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, DirectionDto, object>>
            {
                //ex. { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },

            }, _localizer["Directions"]);
            throw new System.NotImplementedException();
        }
        public async Task<byte[]> Handle(CreateDirectionsTemplateCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportDirectionsCommandHandler method 
            var fields = new string[] {
                   //TODO:Defines the title and order of the fields to be imported's template
                   //_localizer["Name"],
                };
            var result = await _excelService.CreateTemplateAsync(fields, _localizer["Directions"]);
            return result;
        }
    }
}
