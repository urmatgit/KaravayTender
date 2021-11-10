using System;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.DTOs
{
    public class VatDto:IMapFrom<Vat>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Vat, VatDto>().ReverseMap();

        }
        public int Id { get; set; }
        
        public string Name { get; set; }
        public decimal Value { get; set; }
        //public virtual ICollection<NomenclatureDto> Nomenclatures { get; set; }
    }
}
