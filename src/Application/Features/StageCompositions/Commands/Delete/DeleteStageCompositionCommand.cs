using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Caching;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Delete
{
    public class DeleteStageCompositionCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => StageCompositionCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => StageCompositionCacheTokenSource.ResetCacheToken;
    }
    

    public class DeleteStageCompositionCommandHandler : 
                 IRequestHandler<DeleteStageCompositionCommand, Result>
                 
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteStageCompositionCommandHandler> _localizer;
        public DeleteStageCompositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteStageCompositionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteStageCompositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteStageCompositionCommandHandler method 
           var item = await _context.StageCompositions.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.StageCompositions.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        
    }
}
