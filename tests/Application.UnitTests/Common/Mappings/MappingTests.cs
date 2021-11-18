// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.Serialization;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Customers.DTOs;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs;
using CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs;
//using CleanArchitecture.Razor.Application.Documents.DTOs;
//using CleanArchitecture.Razor.Application.DocumentTypes.DTOs;
//using CleanArchitecture.Razor.Application.Features.ApprovalDatas.DTOs;
using CleanArchitecture.Razor.Application.KeyValues.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using NUnit.Framework;

namespace CleanArchitecture.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                //cfg.Advanced.AllowAdditiveTypeMapCreation = true;
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        //[Test]
        //public void ShouldHaveValidConfiguration()
        //{
        //    _configuration.AssertConfigurationIsValid();
        //}

        [Test]
        [TestCase(typeof(StatusLog), typeof(StatusLogDto))]
        [TestCase(typeof(QualityDoc), typeof(QualityDocDto))]
        [TestCase(typeof(Nomenclature), typeof(NomenclatureDto))]
        [TestCase(typeof(Contragent), typeof(ContragentDto))]
        [TestCase(typeof(Category), typeof(CategoryDto))]
        [TestCase(typeof(Direction), typeof(DirectionDto))]
        [TestCase(typeof(Category), typeof(CategoryDto))]
        [TestCase(typeof(Customer), typeof(CustomerDto))]
        [TestCase(typeof(KeyValue), typeof(KeyValueDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}
