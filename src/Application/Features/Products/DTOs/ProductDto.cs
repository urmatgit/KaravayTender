// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.Customers.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.Products.DTOs
{
    public class ProductDto : IMapFrom<Product>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>().ReverseMap();

        }
        //TODO:Define field properties
        public TrackingState TrackingState { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public List<CustomerDto> Customers { get; set; }

    }
}
