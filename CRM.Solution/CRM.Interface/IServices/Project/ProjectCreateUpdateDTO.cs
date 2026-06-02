using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Project
{
    public class ProjectCreateUpdateDTO
    {
        [Required, MaxLength(100)] 
        public string TitleArabic { get; set; } = default!;
        [Required, MaxLength(100)] 
        public string TitleEnglish { get; set; } = default!;
        [MaxLength(500)] 
        public string? DescriptionAr { get; set; }
        [MaxLength(500)]
        public string? DescriptionEn { get; set; }
        [MaxLength(100)] 
        public string? DeveloperName { get; set; }
        [MaxLength(200)] 
        public string? Location { get; set; }
    }
}
