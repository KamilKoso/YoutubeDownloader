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
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required !")]
        public string Password { get; set; }
    }
}
