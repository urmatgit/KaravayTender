using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Contragents.Caching;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Update
{
    public class UpdateContragentCommand: ContragentDto,IRequest<Result>, IMapFrom<Contragent>
    {
         public string CacheKey => ContragentCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ContragentCacheTokenSource.ResetCacheToken;
    }

    public class UpdateContragentCommandHandler : IRequestHandler<UpdateContragentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateContragentCommandHandler> _localizer;
        public UpdateContragentCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateContragentCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateContragentCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateContragentCommandHandler method 
           var item =await _context.Contragents.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
