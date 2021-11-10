using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.Vats.Caching;
using CleanArchitecture.Razor.Application.Features.References.Vats.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.Create
{
    public class CreateVatCommand: VatDto,IRequest<Result<int>>, IMapFrom<Vat>
    {
        public string CacheKey => VatCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => VatCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateVatCommandHandler : IRequestHandler<CreateVatCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateVatCommand> _localizer;
        public CreateVatCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateVatCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateVatCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateVatCommandHandler method 
           var item = _mapper.Map<Vat>(request);
           _context.Vats.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int>.Success(item.Id);
        }
    }
}
