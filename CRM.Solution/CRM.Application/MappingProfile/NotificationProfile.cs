using AutoMapper;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Interface.IServices.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.MappingProfile
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationCreateDTO, Notification>();
            CreateMap<Notification, NotificationReadDTO>()
                .ForMember(d => d.TypeName, o => o.MapFrom(s => s.Type.ToString()));
        }
    }
}
