using System;
using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.DTOs;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using CleanArchitecture.Razor.Application.Features.References.Vats.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs
{
    public class NomenclatureDto:IMapFrom<Nomenclature>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Nomenclature, NomenclatureDto>().ReverseMap();

        }
        public int Id { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// название файла Спецификации
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// Объем Потребление в месяц
        /// </summary>
        public decimal Volume { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
        public int DirectionId { get; set; }
        public virtual DirectionDto Direction { get; set; }
        public int UnitId { get; set; }
        public virtual UnitOfDto Unit { get; set; }
        public int VatId { get; set; }
        public virtual VatDto Vat { get; set; }
        public virtual ICollection<NomenclatureQualityDocDto> NomenclatureQualityDocs { get; set; }
    }
}
