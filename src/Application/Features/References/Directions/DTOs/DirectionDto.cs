using System;
using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Customers.DTOs;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Application.Features.Directions.DTOs
{
    public class DirectionDto:IMapFrom<Direction>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Direction, DirectionDto>().ReverseMap();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CategoryDto> Categories { get; set; }
        public virtual ICollection<CustomerDto> Customers { get; set; }
    }
}
