using AutoMapper;
using CRM.Infrastructure.Entities.RealStateInventory;
using CRM.Interface.IServices.Owner;
using CRM.Interface.IServices.Project;
using CRM.Interface.IServices.Property;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.MappingProfile
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            // Project Mapping
            CreateMap<ProjectCreateUpdateDTO, Project>();
            CreateMap<Project, ProjectReadDTO>()
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore());

            // Property Mapping
            CreateMap<PropertyCreateUpdateDTO, PropertyModel>();
            CreateMap<PropertyModel, PropertyReadDTO>()
                .ForMember(d => d.TypeName, o => o.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.TitleEnglish)) // هيتم تعديله بالـ Localization في السيرفس
                .ForMember(d => d.OwnerName, o => o.MapFrom(s => s.Owner != null ? s.Owner.FullName : null));

            CreateMap<OwnerCreateUpdateDTO, Owner>();
            CreateMap<Owner, OwnerReadDTO>();

        }
    }
}
