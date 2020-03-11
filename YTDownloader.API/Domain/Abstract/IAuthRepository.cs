using System.Threading.Tasks;
using YTDownloader.API.Models;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IAuthRepository
    {
        Task<User> Login(string UsernameOrEmail, string password);
        Task<User> Register(User user, string password);
        Task<bool> UserExists(string username, string email);
    }
}
