using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Delete
{
    public class DeleteComParticipantCommand: IRequest<Result>
    {
      public int ContragentId {  get; set; }
        public int ComOfferId { get; set; }
        public string CacheKey => ComParticipantCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ComParticipantCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedComParticipantsCommand : IRequest<Result>
    {
        public int[] Id { get; set; }
        public int ComOfferId { get; set; }
        public string CacheKey => ComParticipantCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComParticipantCacheTokenSource.ResetCacheToken;
    }

    public class DeleteComParticipantCommandHandler : 
                 IRequestHandler<DeleteComParticipantCommand, Result>,
        IRequestHandler<DeleteCheckedComParticipantsCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteComParticipantCommandHandler> _localizer;
        public DeleteComParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteComParticipantCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteComParticipantCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteComParticipantCommandHandler method 
           var item = await _context.ComParticipants.FindAsync(new object[] {request.ComOfferId, request.ContragentId }, cancellationToken);
            _context.ComParticipants.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedComParticipantsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteCheckedComPositionsCommandHandler method 
            var items = await _context.ComParticipants.Where(x => request.Id.Contains(x.ContragentId) && x.ComOfferId==request.ComOfferId).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.ComParticipants.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
