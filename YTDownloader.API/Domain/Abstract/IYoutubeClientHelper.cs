using System.Threading.Tasks;
using YTDownloader.API.Models;

namespace YTDownloader.API.Domain.Abstract
{
    public interface IYoutubeClientHelper
    {
        public Task<VideoDetails> GetVideoMetadataAsync(string videoId);
        public string GetVideoID(string videoUrl);
        public Task DownloadVideoAsync(string id, string quality, string videoPath);
        public Task DownloadAudioAsync(string id, string audioPath);
    }
}
