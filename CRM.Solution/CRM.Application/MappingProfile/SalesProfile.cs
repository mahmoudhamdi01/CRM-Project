using AutoMapper;
using CRM.Infrastructure.Entities.Deals;
using CRM.Interface.IServices.Deal;
using CRM.Interface.IServices.Installment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.MappingProfile
{
    public class SalesProfile : Profile
    {
        public SalesProfile()
        {
            // Deal
            CreateMap<DealCreateUpdateDTO, Deal>();
            CreateMap<Deal, DealReadDTO>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LeadName, o => o.MapFrom(s => s.Lead.FullName))
                .ForMember(d => d.PropertyCode, o => o.MapFrom(s => s.Property.UnitCode))
                .ForMember(d => d.AssignedUserName, o => o.MapFrom(s => s.AssignedUser != null ? s.AssignedUser.DisplayName : null));

            // Installment
            CreateMap<InstallmentCreateUpdateDTO, Installment>();
            CreateMap<Installment, InstallmentReadDTO>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}
