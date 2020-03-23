using System.Threading.Tasks;
using YTDownloader.API.Models;
using YTDownloader.EFDataAccess.Models;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IAuthRepository
    {
        Task<User> LoginAsync(string usernameOrEmail, string password);
        Task<User> RegisterAsync(User user, string password);
        Task<bool> UserExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> ChangeAccountLevel(string username, AccountLevel accountLevel);



    }
}
