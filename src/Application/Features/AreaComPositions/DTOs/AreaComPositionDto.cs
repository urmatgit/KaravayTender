using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.References.Areas.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.DTOs
{
    public class AreaComPositionDto:IMapFrom<AreaComPosition>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AreaComPosition, AreaComPositionDto>().ReverseMap();

        }
        [Required]
        public int AreaId { get; set; }
        public virtual AreaDto Area { get; set; }
        [Required]
        public int ComPositionId { get; set; }
        public virtual ComPositionDto ComPosition { get; set; }
    }
}
