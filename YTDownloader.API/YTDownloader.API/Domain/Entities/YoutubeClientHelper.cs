using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using YTDownloader.API.Models;

namespace YTDownloader.API.Infrastructure
{
    public class YoutubeClientHelper
    {
        private IYoutubeClient client;

        public YoutubeClientHelper(IYoutubeClient client)
        {
            this.client = client;
        }

        public async Task<VideoDetails> GetVideoMetadata(string videoUrl)
        {
            string id = YoutubeClient.ParseVideoId(videoUrl);
            var video = await client.GetVideoAsync(id);
            MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
            IEnumerable<string> qualities = streamInfoSet.GetAllVideoQualityLabels();

            return new VideoDetails() { id = id, ChannelName = video.Author, Title = video.Title, qualities = qualities, thumbnails = video.Thumbnails };
        }
    }
}
