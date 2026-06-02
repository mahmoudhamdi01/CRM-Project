using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Notification
{
    public class NotificationQueryParams : BaseQueryParams
    {
        public bool? IsRead { get; set; }
    }
}
