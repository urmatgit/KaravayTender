using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Import
{
    public class ImportComStagesCommand: IRequest<Result>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class CreateComStagesTemplateCommand : IRequest<byte[]>
    {
        public IEnumerable<string> Fields { get; set; }
        public string SheetName { get; set; }
    }

    public class ImportComStagesCommandHandler : 
                 IRequestHandler<CreateComStagesTemplateCommand, byte[]>,
                 IRequestHandler<ImportComStagesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportComStagesCommandHandler> _localizer;
        private readonly IExcelService _excelService;

        public ImportComStagesCommandHandler(
            IApplicationDbContext context,
            IExcelService excelService,
            IStringLocalizer<ImportComStagesCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _excelService = excelService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportComStagesCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing ImportComStagesCommandHandler method
           var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ComStageDto, object>>
            {
                //ex. { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },

            }, _localizer["ComStages"]);
           throw new System.NotImplementedException();
        }
        public async Task<byte[]> Handle(CreateComStagesTemplateCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportComStagesCommandHandler method 
            var fields = new string[] {
                   //TODO:Defines the title and order of the fields to be imported's template
                   //_localizer["Name"],
                };
            var result = await _excelService.CreateTemplateAsync(fields, _localizer["ComStages"]);
            return result;
        }
    }
}
