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
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.Export
{
    public class ExportContragentsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportContragentsQueryHandler :
         IRequestHandler<ExportContragentsQuery, byte[]>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportContragentsQueryHandler> _localizer;
        private   List<IApplicationUser> Managers ;
        public ExportContragentsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportContragentsQueryHandler> localizer,
            IIdentityService identityService
            )
        {
            _identityService = identityService;
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }
        private (IApplicationUser,bool) findManager( string id)
        {
            (IApplicationUser, bool) result = (null, false);
            var manager = Managers.FirstOrDefault(m => m.Id == id);
            if (manager != null)
            {
                return (manager, true);
            }
            return result;
        }
        public async Task<byte[]> Handle(ExportContragentsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportContragentsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Contragent>(request.FilterRules);
            var data = await _context.Contragents.Where(filters)
                       .Include(i => i.Direction)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<ContragentDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
              Managers = await _identityService.FetchUsersEx("Manager");
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ContragentDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                    { _localizer["Name"], item => item.Name },
                    { _localizer["FullName"], item => item.FullName },
                    { _localizer["INN"], item => item.INN },
                    { _localizer["KPP"], item => item.KPP },
                    { _localizer["Site"], item => item.Site },
                    { _localizer["Phone"], item => item.Phone },
                    { _localizer["ContactPerson"], item => item.ContactPerson },
                    { _localizer["ContactPhone"], item => item.ContactPhone },
                    { _localizer["Email"], item => item.Email },
                    { _localizer["TypeOfActivity"], item => item.TypeOfActivity },
                    { _localizer["Service"], item => item.IsService?"Да": "Нет"},
                    { _localizer["Status"], item => item.Status.ToDescriptionString() },
                    { _localizer["Manager"], item => {
                        var manger=findManager(item.ManagerId);
                        return manger.Item2? manger.Item1.DisplayName: "";
                        } },
                    { _localizer["ManagerPhone"], item => {
                        var manger=findManager(item.ManagerId);
                        return manger.Item2? manger.Item1.PhoneNumber: "";
                        } },
                       { _localizer["Direction"], item => item.Direction.Name }
                }
                , _localizer["Contragents"]);
            return result;
        }
    }
}

