// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Customers.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Customers.Queries.Export;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Customers.Commands.Import
{
    public class ImportCustomersCommand : IRequest<Result>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class ImportCustomersCommandHandler : IRequestHandler<ImportCustomersCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportCustomersQueryHandler> _localizer;
        private readonly IValidator<AddEditCustomerCommand> _addValidator;

        public ImportCustomersCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportCustomersQueryHandler> localizer,
            IValidator<AddEditCustomerCommand> addValidator
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
            _addValidator = addValidator;
        }
        public async Task<Result> Handle(ImportCustomersCommand request, CancellationToken cancellationToken)
        {
            var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, Customer, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },
                { _localizer["Name Of English"], (row,item) => item.NameOfEnglish = row[_localizer["Name Of English"]]?.ToString() },
                { _localizer["Group Name"], (row,item) => item.GroupName =  row[_localizer["Group Name"]]?.ToString() },
                { _localizer["Partner Type"], (row,item) => item.PartnerType = (PartnerType) Enum.Parse(typeof(PartnerType), row[_localizer["Partner Type"]]==null?"TP":"IC", true)  },
                { _localizer["Region"], (row,item) => item.Region =  row[_localizer["Region"]]?.ToString() },
                { _localizer["Sales"], (row,item) => item.Sales =  row[_localizer["Sales"]]?.ToString() },
                { _localizer["Region Sales Director"], (row,item) => item.RegionSalesDirector =  row[_localizer["Region Sales Director"]]?.ToString() },
                { _localizer["Address"], (row,item) => item.Address =  row[_localizer["Address"]].ToString() },
                { _localizer["Address Of English"], (row,item) => item.AddressOfEnglish =  row[_localizer["Address Of English"]]?.ToString() },
                { _localizer["Contract"], (row,item) => item.Contract =  row[_localizer["Contract"]]?.ToString() },
                { _localizer["Phone Number"], (row,item) => item.PhoneNumber =  row[_localizer["Phone Number"]]?.ToString() },
                { _localizer["Fax"], (row,item) => item.Fax =  row[_localizer["Fax"]]?.ToString() },
                { _localizer["Comments"], (row,item) => item.Comments =  row[_localizer["Comments"]]?.ToString() }
            }, _localizer["Customers"]);

            if (result.Succeeded)
            {
                var importItems = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var item in importItems)
                {
                    var validationResult = await _addValidator.ValidateAsync(_mapper.Map<AddEditCustomerCommand>(item), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        var exist = await _context.Customers.AnyAsync(x => x.Name==item.Name,cancellationToken);
                        if (!exist)
                        {
                            await _context.Customers.AddAsync(item, cancellationToken);
                        }
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(item.Name) ? $"{item.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result.FailureAsync(errors);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailureAsync(result.Errors);
            }
        }
    }
}
