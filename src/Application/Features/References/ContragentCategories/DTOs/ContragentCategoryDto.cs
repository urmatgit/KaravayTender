// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs
{
    public class ContragentCategoryDto : IMapFrom<ContragentCategory>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ContragentCategory, ContragentCategoryDto>().ReverseMap();

        }
        public int ContragentId { get; set; }
        public virtual ContragentDto Contragent { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
    }
}
