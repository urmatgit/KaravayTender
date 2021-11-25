using System;
using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.DTOs
{
    public class ComOfferDto:IMapFrom<ComOffer>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ComOffer, ComOfferDto>().ReverseMap();

        }
        public int Id { get; set; }


        
        public string Name { get; set; }
        
        public LotStatus Status { get; set; } = LotStatus.Preparation;
        /// <summary>
        /// Номер лота
        /// </summary>
        
        public string Number { get; set; }

        
        public DateTime DateBegin { get; set; }
        
        public DateTime DateEnd { get; set; }
        
        public int DirectionId { get; set; }
        public virtual DirectionDto Direction { get; set; }

        
        public DateTime TermBegin { get; set; }
        
        public DateTime TermEnd { get; set; }
        
        public string ManagerId { get; set; }

        /// <summary>
        /// Отсрочка платежа, дней
        /// </summary>
        public short DelayDay { get; set; }
        /// <summary>
        /// Отсрочка в  Банковских днях
        /// </summary>
        public bool IsBankDays { get; set; }


        public int WinnerId { get; set; }
        public virtual Contragent Winner { get; set; }
        /// <summary>
        /// Цена доставки включена в стоимость 
        /// </summary>
        public bool IsPriceInDelivery { get; set; }

        public virtual ICollection<ComParticipantDto> ComParticipants { get; set; }
        public virtual ICollection<ComStageDto> ComStages { get; set; }
        public virtual ICollection<ComPositionDto> ComPositions { get; set; }
    }
}
