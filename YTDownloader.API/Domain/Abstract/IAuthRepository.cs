using System.Threading.Tasks;
using YTDownloader.API.Models;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IAuthRepository
    {
        Task<User> Login(string username, string password);
        Task<User> Register(User user, string password);
        Task<bool> UserExists(string username);
    }
}
