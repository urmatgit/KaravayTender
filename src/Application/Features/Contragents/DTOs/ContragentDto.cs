using System;
using System.Collections.Generic;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.Features.Contragents.DTOs
{
    public class ContragentDto:IMapFrom<Contragent>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Contragent, ContragentDto>().ReverseMap();

        }
        public string ApplicationUserId { get; set; }
        public int Id { get; set; }
       
        public string Name { get; set; }

         
        public string FullName { get; set; }
       
        public string INN { get; set; }
        
        public string KPP { get; set; }
       
        public string Site { get; set; }
       
        public string Phone { get; set; }
        /// <summary>
        /// Контакное лицо
        /// </summary>
       
        public string ContactPerson { get; set; }
        
        public string ContactPhone { get; set; }
       
        public string Email { get; set; }
        /// <summary>
        /// вид деятельности
        /// </summary>
        public string TypeOfActivity { get; set; }
        /// <summary>
        /// Услуга
        /// </summary>
        public bool IsService { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        //public string RegistrationNumber { get; set; }
        //public string Password { get; set; }
        public ContragentStatus Status { get; set; }

        public int DirectionId { get; set; }
        public virtual DirectionDto Direction { get; set; }
        public virtual ICollection<ContragentCategoryDto> ContragentCategories { get; set; }
    }
}
