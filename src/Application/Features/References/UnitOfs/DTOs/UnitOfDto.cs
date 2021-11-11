// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs
{
    public class UnitOfDto : IMapFrom<UnitOf>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UnitOf, UnitOfDto>().ReverseMap();

        }
        public int Id { get; set; }

        [Required(ErrorMessage = "'Наименование' является обязательным ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "'Полное наименование' является обязательным")]
        public string FullName { get; set; }
        // public virtual ICollection<NomenclatureDto> Nomenclatures { get; set; }
    }
}
