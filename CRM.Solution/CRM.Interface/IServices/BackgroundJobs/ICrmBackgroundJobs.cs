using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.BackgroundJobs
{
    public interface ICrmBackgroundJobs
    {
        Task ProcessOverdueInstallmentsAsync();
        Task ProcessDailyFollowUpRemindersAsync();
    }
}
