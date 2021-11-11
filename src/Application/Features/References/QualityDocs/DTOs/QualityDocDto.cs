using System;
using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs
{
    public class QualityDocDto:IMapFrom<QualityDoc>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<QualityDoc, QualityDocDto>().ReverseMap();

        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }
        //public virtual ICollection<NomenclatureQualityDocDto> NomenclatureQualityDocs { get; set; }
    }
}
