using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using YTDownloader.API.Models;
using YoutubeExplode.Converter;
using YTDownloader.API.Domain.Abstract;
using System.IO;
using System;

namespace YTDownloader.API.Domain.Entities
{
    public class YoutubeClientHelper:IYoutubeClientHelper
    {
        IYoutubeClient client;
        readonly IWebHostEnvironment env;
        IYoutubeConverter converter;

        public YoutubeClientHelper(IYoutubeClient client, IWebHostEnvironment env)
        {
            this.env = env;
            this.client = client;

            string ffmpegExePath = env.WebRootPath + "\\ffmpeg.exe"; //Path to the ffmpeg.exe file used to mux audio&video stream. It should be located in wwwrooot/ffmpeg.exe
            converter = new YoutubeConverter(client, ffmpegExePath);
            
        }

        public string GetVideoID(string videoUrl)
        {
            return YoutubeClient.ParseVideoId(videoUrl);
        }

        public async Task<VideoDetails> GetVideoMetadata(string videoId)
        {
            var video = await client.GetVideoAsync(videoId);
            MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(videoId);
            IEnumerable<string> qualities = SortQualities(streamInfoSet.GetAllVideoQualityLabels());

            return new VideoDetails() { id = videoId, ChannelName = video.Author, Title = video.Title, qualities = qualities, thumbnails = video.Thumbnails };
        }

        public async Task DownloadVideo(string id, string quality, string videoPath)
        {
            MediaStreamInfoSet streamInfoSet;
            streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
            var audioStreamInfo = streamInfoSet.Audio.WithHighestBitrate();
            var videoStreamInfo = streamInfoSet.Video.FirstOrDefault(c => c.VideoQualityLabel == quality);
            var mediaStreamInfos = new MediaStreamInfo[] { audioStreamInfo, videoStreamInfo };
            await converter.DownloadAndProcessMediaStreamsAsync(mediaStreamInfos, videoPath, "mp4");
        }

         IEnumerable<string> SortQualities(IEnumerable<string> qualities)
        {
            List<string> sortedStrings = qualities.ToList();
            return sortedStrings
                .Select(s => new { str = s, split = s.Split('p') })
                .OrderBy(x => int.Parse(x.split[0]))
                .ThenBy(x => x.split[1])
                .Select(x => x.str)
                .ToList();
        }
    }
}
