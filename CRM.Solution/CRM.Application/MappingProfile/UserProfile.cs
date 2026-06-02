using AutoMapper;
using CRM.Infrastructure.Entities.IdentityModule;
using CRM.Interface.IServices.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateUpdateDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)); // Identity requires UserName

            CreateMap<ApplicationUser, UserReadDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.DisplayName : null));
        }
    }
}
