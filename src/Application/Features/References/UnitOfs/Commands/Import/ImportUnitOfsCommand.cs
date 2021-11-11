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
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Import
{
    public class ImportUnitOfsCommand : IRequest<Result>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class CreateUnitOfsTemplateCommand : IRequest<byte[]>
    {
        public IEnumerable<string> Fields { get; set; }
        public string SheetName { get; set; }
    }

    public class ImportUnitOfsCommandHandler :
                 IRequestHandler<CreateUnitOfsTemplateCommand, byte[]>,
                 IRequestHandler<ImportUnitOfsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportUnitOfsCommandHandler> _localizer;
        private readonly IExcelService _excelService;

        public ImportUnitOfsCommandHandler(
            IApplicationDbContext context,
            IExcelService excelService,
            IStringLocalizer<ImportUnitOfsCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _excelService = excelService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportUnitOfsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportUnitOfsCommandHandler method
            var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, UnitOfDto, object>>
            {
                //ex. { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },

            }, _localizer["UnitOfs"]);
            throw new System.NotImplementedException();
        }
        public async Task<byte[]> Handle(CreateUnitOfsTemplateCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportUnitOfsCommandHandler method 
            var fields = new string[] {
                   //TODO:Defines the title and order of the fields to be imported's template
                   //_localizer["Name"],
                };
            var result = await _excelService.CreateTemplateAsync(fields, _localizer["UnitOfs"]);
            return result;
        }
    }
}
