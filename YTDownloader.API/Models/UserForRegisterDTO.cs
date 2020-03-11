using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YTDownloader.API.Models
{
    public class UserForRegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
