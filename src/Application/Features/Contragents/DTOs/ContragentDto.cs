using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
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
            //profile.CreateMap<Contragent, ContragentDto>().ReverseMap();
            profile.CreateMap<Contragent, ContragentDto>()
                .ForMember(x => x.DirectionName, s => s.MapFrom(y => y.Direction.Name));
            profile.CreateMap<Contragent, ContragentDto>()
                .ForMember(x => x.StatusStr, s => s.MapFrom(y => y.Status.ToDescriptionString()));
            profile.CreateMap<ContragentDto, Contragent>(MemberList.None);

        }
        public string ApplicationUserId { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "'Наименование' является обязательным ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "'Полное наименование' является обязательным")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "'ИНН' является обязательным")]
        public string INN { get; set; }
        
        public string KPP { get; set; }
       
        public string Site { get; set; }

        [Required(ErrorMessage = "'Телефон организации' является обязательным")]
        public string Phone { get; set; }
        /// <summary>
        /// Контакное лицо
        /// </summary>
        [Required(ErrorMessage = "'Контакное лицо' является обязательным")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "'Моб. тел. контакного лица' является обязательным")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "'Адрес электроной почты' является обязательным")]
        public string Email { get; set; }
        /// <summary>
        /// вид деятельности
        /// </summary>
        [Required(ErrorMessage = "'Вид деятельности' является обязательным")]
        public string TypeOfActivity { get; set; }
        /// <summary>
        /// Услуга
        /// </summary>
        public bool IsService { get; set; }
        public bool IsServiceStr
        {
            get
            {
                return IsService;
            }
        }
        [Required(ErrorMessage = "Не выбран 'Менеджер'")]
        public string ManagerId { get; set; }
        public string ManagerPhone { get; set; }
        public string Manager { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        //public string RegistrationNumber { get; set; }
        //public string Password { get; set; }
        public ContragentStatus Status { get; set; } = ContragentStatus.OnRegistration;
        public string StatusStr { get; set; } = ContragentStatus.OnRegistration.ToDescriptionString();
        [Required(ErrorMessage = "Не выбрано 'Направление'")]
        public int DirectionId { get; set; }
        [Required(ErrorMessage = "Не выбрано 'Направление'")]
        public virtual DirectionDto Direction { get; set; }
        public string DirectionName { get; set; }

        public DateTime Created { get; set; }
        public virtual ICollection<ContragentCategoryDto> ContragentCategories { get; set; }
    }
}
