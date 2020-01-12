using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTDownloader.API.Models;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IYoutubeClientHelper
    {
        public Task<VideoDetails> GetVideoMetadata(string videoId);
        public string GetVideoID(string videoUrl);
        public Task DownloadVideo(string id, string quality, string videoPath);
        public Task DownloadAudio(string id, string audioPath);
    }
}
