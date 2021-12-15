using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.DTOs
{
    public class StageParticipantDto:IMapFrom<StageParticipant>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StageParticipant, StageParticipantDto>().ReverseMap();

        }
        public int ComStageId { get; set; }
        public virtual ComStageDto ComStage { get; set; }
        [Required]
        public int ContragentId { get; set; }
        public virtual ContragentDto Contragent { get; set; }

        [Required]
        public int ComOfferId { get; set; }
        public virtual ComOfferDto ComOffer { get; set; }
    }
}
