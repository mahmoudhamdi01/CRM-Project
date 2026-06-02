using AutoMapper;
using CRM.Application.Services;
using CRM.Infrastructure.Entities.IdentityModule;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Authentication;
using CRM.Interface.IServices.Dashboard;
using CRM.Interface.IServices.Deal;
using CRM.Interface.IServices.Installment;
using CRM.Interface.IServices.Lead;
using CRM.Interface.IServices.LeadInteraction;
using CRM.Interface.IServices.LeadSource;
using CRM.Interface.IServices.Notification;
using CRM.Interface.IServices.Owner;
using CRM.Interface.IServices.Project;
using CRM.Interface.IServices.Property;
using CRM.Interface.IServices.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Repositories
{
    public class ServiceManager(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper,
        IEntityAuditHelper entityAuditHelper, ILocalizationService localizationService,
        IUnitOfWork unitOfWork) : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _LazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, configuration));
        private readonly Lazy<IUserService> _LazyUserService = new Lazy<IUserService>(() => new UserService(userManager, mapper, entityAuditHelper));
        private readonly Lazy<IPropertyService> _LazyPropertyService = new Lazy<IPropertyService>(() => new PropertyService(unitOfWork, mapper, localizationService, entityAuditHelper));
        private readonly Lazy<IProjectService> _LazyProjectService = new Lazy<IProjectService>(() => new ProjectService(unitOfWork, mapper, localizationService, entityAuditHelper));
        private readonly Lazy<IOwnerService> _LazyOwnerService = new Lazy<IOwnerService>(() => new OwnerService(unitOfWork, mapper, entityAuditHelper));
        private readonly Lazy<ILeadService> _LazyLeadService = new Lazy<ILeadService>(() => new LeadService(unitOfWork, mapper,localizationService, entityAuditHelper));
        private readonly Lazy<ILeadSourceService> _LazyLeadSourceService = new Lazy<ILeadSourceService>(() => new LeadSourceService(unitOfWork, mapper,localizationService, entityAuditHelper));
        private readonly Lazy<ILeadInteractionService> _LazyLeadInteractionService = new Lazy<ILeadInteractionService>(() => new LeadInteractionService(unitOfWork, mapper, entityAuditHelper));
        private readonly Lazy<IDealService> _lazyDealService = new Lazy<IDealService>(() => new DealService(unitOfWork, mapper, entityAuditHelper));
        private readonly Lazy<IInstallmentService> _LazyInstallmentService = new Lazy<IInstallmentService>(() => new InstallmentService(unitOfWork, mapper, entityAuditHelper));
        private readonly Lazy<INotificationService> _LazyNotificationService = new Lazy<INotificationService>(() => new NotificationService(unitOfWork, mapper, entityAuditHelper));
        private readonly Lazy<IDashboardService> _LazyDashboardService = new Lazy<IDashboardService>(() => new DashboardService(unitOfWork));

        public IAuthenticationService AuthenticationService => _LazyAuthenticationService.Value;
        public IUserService UserService => _LazyUserService.Value;
        public IPropertyService PropertyService => _LazyPropertyService.Value;
        public IProjectService ProjectService => _LazyProjectService.Value;
        public IOwnerService OwnerService => _LazyOwnerService.Value;
        public ILeadService LeadService => _LazyLeadService.Value;
        public ILeadSourceService LeadSourceService => _LazyLeadSourceService.Value;
        public ILeadInteractionService LeadInteractionService => _LazyLeadInteractionService.Value;
        public IDealService DealService => _lazyDealService.Value;
        public IInstallmentService InstallmentService => _LazyInstallmentService.Value;
        public IDashboardService DashboardService => _LazyDashboardService.Value;
        public INotificationService NotificationService => _LazyNotificationService.Value;
    }
}
