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

        [Required(ErrorMessage = "Не указано наименование")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указано полное наименование")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Не указано ИНН")]
        public string INN { get; set; }
        
        public string KPP { get; set; }
       
        public string Site { get; set; }

        [Required(ErrorMessage = "'Телефон организации' должно быть заполнено")]
        public string Phone { get; set; }
        /// <summary>
        /// Контакное лицо
        /// </summary>
        [Required(ErrorMessage = "'Контакное лицо' должно быть заполнено")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "'Моб. тел. контакного лица' должно быть заполнено")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "'Адрес электроной почты' должно быть заполнено")]
        public string Email { get; set; }
        /// <summary>
        /// вид деятельности
        /// </summary>
        [Required(ErrorMessage = "'Вид деятельности' должно быть заполнено")]
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
        public string ManagerId { get; set; }

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
        
        public virtual ICollection<ContragentCategoryDto> ContragentCategories { get; set; }
    }
}
