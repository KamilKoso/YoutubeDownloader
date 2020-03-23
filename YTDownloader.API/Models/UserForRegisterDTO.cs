using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YTDownloader.API.Models
{
    public class UserForRegisterDTO
    {
        [Required(ErrorMessage = "Username is required !")]
        [StringLength(15,MinimumLength = 3, ErrorMessage = "Username must be 3-15 characters long !")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required !")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be 6-15 characters long !")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email address is required !")]
        [StringLength(320, MinimumLength = 3, ErrorMessage = "Invalid email address !")]
        public string Email { get; set; }
    }
}
