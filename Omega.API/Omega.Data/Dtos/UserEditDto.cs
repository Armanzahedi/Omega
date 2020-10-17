using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Omega.Infrastructure.Dtos
{
    public class UserEditDto
    {
        [EmailAddress(ErrorMessage = "ایمیل نا معتبر.")]
        [Required(ErrorMessage = "ایمیل را وارد کنید.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "نام کاربری را وارد کنید.")]
        public string UserName { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
