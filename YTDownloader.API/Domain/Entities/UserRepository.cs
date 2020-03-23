using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Domain.Concrete;
using YTDownloader.API.Models;
using YTDownloader.EFDataAccess.Models;

namespace YTDownloader.API.Domain.Entities
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext context;
        public UserRepository(UserContext context)
        {
            this.context = context;
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<AccountLevel> GetUserAccountLevelAsync(string username)
        {
            var user = await GetUserAsync(username);
            if (user != null)
                return user.UserAccountLevel;
            else
                return AccountLevel.None;
        }
    }
}
