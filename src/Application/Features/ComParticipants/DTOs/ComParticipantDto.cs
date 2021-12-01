using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs
{
    public class ComParticipantDto:IMapFrom<ComParticipant>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ComParticipant, ComParticipantDto>().ReverseMap();

        }
        /// <summary>
        /// Этап отказа
        /// </summary>
        public int StepFailure { get; set; }
        public ParticipantStatus Status { get; set; } = ParticipantStatus.NotParticipate;
        public string StatusStr=> Status.ToDescriptionString();
        [Required]
        public int ContragentId { get; set; }
        public virtual ContragentDto Contragent { get; set; }
        public string ContragentName => Contragent?.Name;
        [Required]
        public int ComOfferId { get; set; }
        public virtual ComOfferDto ComOffer { get; set; }
    }
}
