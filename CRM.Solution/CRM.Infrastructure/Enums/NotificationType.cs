using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Enums
{
    public enum NotificationType
    {
        FollowUpReminder = 1,  // تذكير بموعد متابعة عميل
        NewLeadAssigned = 2,   // تم تعيين عميل جديد للموظف
        DealApproved = 3,      // تم الموافقة على صفقة
        InstallmentOverdue = 4 // قسط متأخر
    }
}
