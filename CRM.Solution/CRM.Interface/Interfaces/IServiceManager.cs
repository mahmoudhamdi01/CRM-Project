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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.Interfaces
{
    public interface IServiceManager
    {
        public IAuthenticationService AuthenticationService { get; }
        public IUserService UserService { get; }
        public IPropertyService PropertyService { get; }
        public IProjectService ProjectService { get; }
        public IOwnerService OwnerService { get; }
        public ILeadService LeadService { get; }
        public ILeadSourceService LeadSourceService { get; }
        public ILeadInteractionService LeadInteractionService { get; }
        public IDealService DealService { get; }
        public IInstallmentService InstallmentService { get; }
        public IDashboardService DashboardService { get; }
        public INotificationService NotificationService { get; }
    }
}
