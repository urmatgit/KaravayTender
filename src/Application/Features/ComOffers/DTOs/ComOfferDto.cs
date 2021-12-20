using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Identity;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.DTOs
{
    public class ComOfferDto:IMapFrom<ComOffer>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ComOfferDto, ComOffer>()
                .ForMember(d => d.TermBegin, s => s.MapFrom(y => y.TermBegin == null ? default(DateTime) : y.TermBegin))
                .ForMember(d => d.TermEnd, s => s.MapFrom(y => y.TermEnd == null ? default(DateTime) : y.TermEnd));
            profile.CreateMap<ComOffer, ComOfferDto>()
                .ForMember(d => d.TermBegin, s => s.MapFrom(x => x.TermBegin == default(DateTime) ? default(DateTime?) : x.TermBegin))
                .ForMember(d => d.TermEnd, s => s.MapFrom(x => x.TermEnd == default(DateTime) ? default(DateTime?) : x.TermEnd));

        }
        public int Id { get; set; }


        [Required(ErrorMessage = "'Наименование' является обязательным ")]
        public string Name { get; set; }
        
        public ComOfferStatus Status { get; set; } = ComOfferStatus.Preparation;
        public  string StatusStr=>Status.ToDescriptionString();
        /// <summary>
        /// Номер лота
        /// </summary>
        [Required(ErrorMessage = "'№ ком. предложения' является обязательным ")]
        public string Number { get; set; }

        
        public DateTime DateBegin { get; set; }
        
        public DateTime? DateEnd { get; set; }
        [Required]
        public int DirectionId { get; set; }
        public virtual DirectionDto Direction { get; set; }

        public string DirectionName => Direction?.Name;

        [Required(ErrorMessage = "'Срок контракта с' является обязательным ")]
        public DateTime? TermBegin { get; set; }

        [Required(ErrorMessage = "'Срок контракта по' является обязательным ")]
        public DateTime? TermEnd { get; set; }
        [Required(ErrorMessage = "Не выбрано 'Менеджер'")]
        public string ManagerId { get; set; }
        public virtual ApplicationUser Manager { get; set; }
        public string ManagerName => Manager?.DisplayName ?? Manager?.UserName;
        [Required(ErrorMessage = "'Отсрочка платежа' является обязательным ")]
        /// <summary>
        /// Отсрочка платежа, дней
        /// </summary>
        public short DelayDay { get; set; }
        /// <summary>
        /// Отсрочка в  Банковских днях
        /// </summary>
        public bool IsBankDays { get; set; }
        public bool IsBankDaysStr => IsBankDays;


        public int? WinnerId { get; set; }
        public virtual ContragentDto Winner { get; set; }
        public string WinnerName
        {
            get
            {
                return Winner?.Name;
            }
        }
        /// <summary>
        /// Цена доставки включена в стоимость 
        /// </summary>
        public bool IsDeliveryInPrice { get; set; }
        public bool IsDeliveryInPriceStr {
            get
            {
                return IsDeliveryInPrice;
            }
        }

        public virtual ICollection<ComParticipantDto> ComParticipants { get; set; }
        public virtual ICollection<ComStageDto> ComStages { get; set; }
        public virtual ICollection<ComPositionDto> ComPositions { get; set; }
    }
}
