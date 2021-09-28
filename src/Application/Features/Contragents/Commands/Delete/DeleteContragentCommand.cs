using System.Linq;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Delete
{
    public class DeleteContragentCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => ContragentCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ContragentCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedContragentsCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => ContragentCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ContragentCacheTokenSource.ResetCacheToken;
    }

    public class DeleteContragentCommandHandler : 
                 IRequestHandler<DeleteContragentCommand, Result>,
                 IRequestHandler<DeleteCheckedContragentsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteContragentCommandHandler> _localizer;
        public DeleteContragentCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteContragentCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteContragentCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteContragentCommandHandler method 
           var item = await _context.Contragents.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Contragents.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedContragentsCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedContragentsCommandHandler method 
           var items = await _context.Contragents.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.Contragents.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
