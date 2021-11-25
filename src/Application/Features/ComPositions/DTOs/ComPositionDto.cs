using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.AreaComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.DTOs
{
    public class ComPositionDto:IMapFrom<ComPosition>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ComPosition, ComPositionDto>().ReverseMap();

        }
        public int Id { get; set; }
        /// <summary>
        /// Количество Поставок в месяц 
        /// </summary>
        [Required]
        public int DeliveryCount { get; set; }
        /// <summary>
        /// Объем  поставок по контракту
        /// </summary>
        [Required]
        public decimal Volume { get; set; }
        public string AddRequirement { get; set; }

        /// <summary>
        /// Цена без ндс
        /// </summary>
        [Required]
        public decimal Price { get; set; }
        /// <summary>
        /// Сумма без НДС
        /// </summary>
        [Required]
        public decimal Summa { get; set; }
        /// <summary>
        /// Сумма с НДС
        /// </summary>
        [Required]
        public decimal SummaVAT { get; set; }

        [Required]
        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<AreaComPositionDto> AreaComPositions { get; set; }
        [Required]
        public int ComOfferId { get; set; }
        public virtual ComOfferDto ComOffer { get; set; }

        public virtual ICollection<StageCompositionDto> StageCompositions { get; set; }
    }
}
