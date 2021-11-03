using System;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs
{
    public class StatusLogDto:IMapFrom<StatusLog>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StatusLog, StatusLogDto>().ReverseMap();

        }
        //TODO:Define field properties
        public int Id { get; set; }
        public int ContragentId { get; set; }
        public virtual ContragentDto Contragent { get; set; }
        public DateTime DateTime { get; set; }
        public ContragentStatus Status { get; set; }
        public string ApplicationUserId { get; set; }
        
    }
}
