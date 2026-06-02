using AutoMapper;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Interface.IServices.Lead;
using CRM.Interface.IServices.LeadInteraction;
using CRM.Interface.IServices.LeadSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.MappingProfile
{
    public class LeadProfile : Profile
    {
        public LeadProfile()
        {
            // Source
            CreateMap<LeadSourceCreateUpdateDTO, LeadSource>();
            CreateMap<LeadSource, LeadSourceReadDTO>()
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore());

            // Lead
            CreateMap<LeadCreateUpdateDTO, Lead>();
            CreateMap<Lead, LeadReadDTO>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.SourceName, o => o.MapFrom(s => s.Source.TitleEnglish)) // هيتم تعديله بالـ Localization في السيرفس
                .ForMember(d => d.AssignedUserName, o => o.MapFrom(s => s.AssignedUser != null ? s.AssignedUser.DisplayName : null));

            // Interaction
            CreateMap<LeadInteractionCreateUpdateDTO, LeadInteraction>();
            CreateMap<LeadInteraction, LeadInteractionReadDTO>()
                .ForMember(d => d.TypeName, o => o.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.LeadName, o => o.MapFrom(s => s.Lead.FullName));
        }
    }
}
