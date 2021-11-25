using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs
{
    public class StageCompositionDto:IMapFrom<StageComposition>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StageComposition, StageCompositionDto>().ReverseMap();

        }
        [Required]
        public int ComStageId { get; set; }
        public virtual ComStageDto ComStage { get; set; }
        [Required]
        public int ContragentId { get; set; }
        public virtual ContragentDto Contragent { get; set; }
        [Required]
        public int ComPositionId { get; set; }
        public virtual ComPositionDto ComPosition { get; set; }
    }
}
