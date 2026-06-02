using CRM.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.Interfaces
{
    public interface ILocalizationService
    {
        string GetCurrentLanguage();
        string GetLocalizedTitle(LocalizationEntity entity);
        string GetLocalizedDescription(LocalizationEntity entity);
        List<string> GetLocalizedList(List<string>? ar, List<string>? en);
    }
}
