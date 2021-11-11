using System;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.DTOs
{
    public class NomenclatureQualityDocDto:IMapFrom<NomenclatureQualityDoc>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<NomenclatureQualityDoc, NomenclatureQualityDocDto>().ReverseMap();

        }
        public int NomenclatureId { get; set; }
       // public virtual NomenclatureDto Nomenclature { get; set; }
        public int QualityDocId { get; set; }
        public virtual QualityDocDto QualityDoc { get; set; }
        
    }
}
