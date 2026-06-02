using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.User
{
    public class UserCreateUpdateDTO
    {
        [Required, MaxLength(150)]
        public string DisplayName { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required, Phone]
        public string PhoneNumber { get; set; } = default!;

        public UserRole Role { get; set; }

        public string? ManagerId { get; set; }

        // بنحتاجه في الكريت فقط، وممكن يكون فاضي في الابديت
        public string? Password { get; set; }
    }
}
