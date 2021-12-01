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
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Import
{
    public class AddContragentsComParticipantCommand: IRequest<Result>
    {

        public int ComOfferId { get; set; }
        public int [] ContragentIds { get; set; }
    }
    
    public class AddContragentsComParticipantCommandHandler : 
                 IRequestHandler<AddContragentsComParticipantCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddContragentsComParticipantCommandHandler> _localizer;
        

        public AddContragentsComParticipantCommandHandler(
            IApplicationDbContext context,
        
            IStringLocalizer<AddContragentsComParticipantCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
           
            _mapper = mapper;
        }
        public async Task<Result> Handle(AddContragentsComParticipantCommand request, CancellationToken cancellationToken)
        {
            List<ComParticipant> newComPart = new List<ComParticipant>();
            foreach (int contragentId in request.ContragentIds)
            {
                ComParticipant participant = new ComParticipant
                {
                    ComOfferId = request.ComOfferId,
                    ContragentId = contragentId
                };
                newComPart.Add(participant);
            }
            _context.ComParticipants.AddRange(newComPart);
            await _context.SaveChangesAsync(cancellationToken);
            

            return  Result.Success();
        }
        
    }
}
