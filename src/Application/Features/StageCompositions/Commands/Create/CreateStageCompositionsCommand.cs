using System.Collections.Generic;
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
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Create
{
    public class CreateStageCompositionsCommand: IRequest<Result>
    {
        public int ComOfferId { get; set; }
        public int ComStageId { get; set; }
    }
    
    public class CreateStageCompositionsCommandHandler : IRequestHandler<CreateStageCompositionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateStageCompositionsCommand> _localizer;
        public CreateStageCompositionsCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateStageCompositionsCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateStageCompositionsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CreateStageCompositionsCommandHandler method 
            var comoffer = await _context.ComOffers
                   .Include(c => c.ComPositions)
                   .Include(c => c.ComParticipants)
                   .Where(c => c.Id == request.ComOfferId)
                   .FirstOrDefaultAsync(cancellationToken);
            if (comoffer != null)
            {
                List<StageComposition> stageComs = new List<StageComposition>();
                foreach (ComPosition position in comoffer.ComPositions)
                {
                    foreach (ComParticipant participant in comoffer.ComParticipants)
                    {
                        var StageCom = new StageComposition()
                        {
                            ComPositionId = position.Id,
                            ContragentId = participant.ContragentId,
                            ComStageId = request.ComStageId,
                            Status = true,
                            Price = 0
                        };
                        //dc.ComStageId, dc.ContragentId,dc.ComPositionId
                        var exist = await _context.StageCompositions.FindAsync(new object[] { request.ComStageId,participant.ContragentId,position.Id },cancellationToken);
                        if (exist==null)
                            stageComs.Add(StageCom);
                    }
                }
                if (stageComs.Count > 0)
                {
                    await _context.StageCompositions.AddRangeAsync(stageComs, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            return Result.Success();
        }
    }
}
