using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComStages.DTOs
{
    public class ComStageDto:IMapFrom<ComStage>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ComStage, ComStageDto>().ReverseMap();

        }
        public int Id { get; set; }
        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Required]
        public int Number { get; set; }
        /// <summary>
        /// Срок предоставления ответа,дни
        /// </summary>
        [Required]
        public int Deadline { get; set; }
        public int ComOfferId { get; set; }
        public virtual ComOfferDto ComOffer { get; set; }
        //[JsonIgnore]
        public virtual ICollection<StageCompositionDto> StageCompositions { get; set; }
    }
}
