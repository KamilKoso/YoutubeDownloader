using System.ComponentModel.DataAnnotations;
using YTDownloader.EFDataAccess.Models;

namespace YTDownloader.API.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        [MinLength(3)]
        public string Username { get; set; }
        [Required]
        [MaxLength(320)] // Max characters: {64}@{255} 64 + 1 + 255 = 320
        public string EmailAddress { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public AccountLevel UserAccountLevel { get; set; }
    }
}
