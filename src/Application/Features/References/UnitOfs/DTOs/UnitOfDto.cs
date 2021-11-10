using System;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs
{
    public class UnitOfDto:IMapFrom<UnitOf>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UnitOf, UnitOfDto>().ReverseMap();

        }
        public int Id { get; set; }
        
        public string Name { get; set; }
      // public virtual ICollection<NomenclatureDto> Nomenclatures { get; set; }
    }
}
