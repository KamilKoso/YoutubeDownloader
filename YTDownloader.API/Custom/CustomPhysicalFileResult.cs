using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using YTDownloader.API.Domain.Entities;

namespace YTDownloader.API.Custom
{
    public class PhysicalFileResultAndDelete : PhysicalFileResult
    {
        public PhysicalFileResultAndDelete(string fileName, string contentType)
                : base(fileName, contentType) { }
        public PhysicalFileResultAndDelete(string fileName, MediaTypeHeaderValue contentType)
                     : base(fileName, contentType) { }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await base.ExecuteResultAsync(context);
            await CleanDirectory.DeleteFile(FileName);
        }
    }
}
