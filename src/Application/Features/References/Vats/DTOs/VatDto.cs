// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.DTOs
{
    public class VatDto : IMapFrom<Vat>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Vat, VatDto>()
                .ForMember(m => m.Stavka_str, s => s.MapFrom(d => d.Stavka.ToString()));
            profile.CreateMap<VatDto, Vat>(MemberList.None);

        }
        public int Id { get; set; }

        [Required(ErrorMessage = "'Наименование' является обязательным ")]
        public string Name { get; set; }


        public string Description { get; set; }
        [Required(ErrorMessage = "'НДС,%' является обязательным ")]

        public decimal Stavka { get; set; }
        [Required(ErrorMessage = "'НДС,%' является обязательным")]
        [RegularExpression(@"^(0*100{1,1}\.?((?<=\.)0*)?%?$)|(^0*\d{0,2}\.?((?<=\.)\d*)?%?)$", ErrorMessage = "Недопустимый процент")]
        public string Stavka_str { get; set; }
        //public virtual ICollection<NomenclatureDto> Nomenclatures { get; set; }
    }
}
