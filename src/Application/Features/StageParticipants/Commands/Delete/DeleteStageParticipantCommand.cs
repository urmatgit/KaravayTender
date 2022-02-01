using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StageParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.StageParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Delete
{
    public class DeleteStageParticipantCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => StageParticipantCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => StageParticipantCacheTokenSource.ResetCacheToken;
    }
    //public class DeleteCheckedStageParticipantsCommand : IRequest<Result>
    //{
    //  public int[] Id {  get; set; }
    //   public string CacheKey => StageParticipantCacheKey.GetAllCacheKey;

    //   public CancellationTokenSource ResetCacheToken => StageParticipantCacheTokenSource.ResetCacheToken;
    //}

    public class DeleteStageParticipantCommandHandler : 
                 IRequestHandler<DeleteStageParticipantCommand, Result>
              
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteStageParticipantCommandHandler> _localizer;
        public DeleteStageParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteStageParticipantCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteStageParticipantCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteStageParticipantCommandHandler method 
           var item = await _context.StageParticipants.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.StageParticipants.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        //public async Task<Result> Handle(DeleteCheckedStageParticipantsCommand request, CancellationToken cancellationToken)
        //{
        //   //TODO:Implementing DeleteCheckedStageParticipantsCommandHandler method 
        //   var items = await _context.StageParticipants.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //    foreach (var item in items)
        //    {
        //        _context.StageParticipants.Remove(item);
        //    }
        //    await _context.SaveChangesAsync(cancellationToken);
        //    return Result.Success();
        //}
    }
}
