using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTDownloader.API.Models;
using YTDownloader.EFDataAccess.Models;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username);
        Task<AccountLevel> GetUserAccountLevelAsync(string username);
    }
}
