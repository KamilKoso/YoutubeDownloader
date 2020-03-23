using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTDownloader.EFDataAccess.Models;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IAccountPermissionChecker
    {
        public Task<(bool isAllowed, string errorMessageIfNotAllowed)> CanDownloadInCertainQualityAsync(string quality, bool isAuthenticated, string username);
    }
}
