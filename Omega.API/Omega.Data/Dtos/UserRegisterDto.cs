using System;
using System.ComponentModel.DataAnnotations;

namespace Omega.Infrastructure.Dtos
{
    public class UserRegisterDto
    {
        [EmailAddress(ErrorMessage ="ایمیل نا معتبر.")]
        [Required(ErrorMessage = "ایمیل را وارد کنید.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "نام کاربری را وارد کنید.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "پسورد را وارد کنید.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
         ErrorMessage = "پسور باید بیشتر از 8 کارکتر بوده و حداقل شامل یک حرف بزرگ یک حرف کوچک یک عدد و یک کارکتر خاص باشد.")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;

    }
}
