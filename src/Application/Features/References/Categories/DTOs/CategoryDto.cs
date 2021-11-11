// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
//using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Application.Features.Categories.DTOs
{
    public class CategoryDto : IMapFrom<Category>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryDto>()
                .ForMember(x => x.DirectionName, s => s.MapFrom(y => y.Direction.Name));
            profile.CreateMap<CategoryDto, Category>(MemberList.None);


        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DirectionId { get; set; }
        public virtual DirectionDto Direction { get; set; }
        public string DirectionName { get; set; }
        public bool IsCheck { get; set; }
        public virtual ICollection<ContragentCategoryDto> ContragentCategories { get; set; }
    }
}
