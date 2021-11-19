using System;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.DTOs
{
    public class AreaDto:IMapFrom<Area>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Area, AreaDto>().ReverseMap();

        }
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Address { get; set; }
        public string Description { get; set; }
    }
}
