using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YTDownloader.API.Models
{
    public class UserForLoginDTO
    {
        [Required(ErrorMessage = "Username or email is required to login !")]
        [StringLength(320, MinimumLength = 3, ErrorMessage = "Username must be 3-15 characters long !")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required !")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password is incorrect ")]
        public string Password { get; set; }
    }
}
