using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Models.MediaStreams;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Models;

namespace YTDownloader.API.Domain.Entities
{
    public class YoutubeClientHelper:IYoutubeClientHelper
    {
        readonly IYoutubeClient client;
        readonly IYoutubeConverter converter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">YoutubeClient instance</param>
        /// <param name="ffmpegExePath">//Path to the ffmpeg.exe file used to mux audio&video stream. It should be located in wwwrooot/ffmpeg.exe</param>
        public YoutubeClientHelper(IYoutubeClient client, string ffmpegExePath)
        {
          
            this.client = client;

            
            converter = new YoutubeConverter(client, ffmpegExePath);
            
        }

        public string GetVideoID(string videoUrl)
        {
            return YoutubeClient.ParseVideoId(videoUrl);
        }

        public async Task<VideoDetails> GetVideoMetadata(string videoID)
        {
            var video = await client.GetVideoAsync(videoID);
            MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(videoID);
            IEnumerable<string> qualities = SortQualities(streamInfoSet.GetAllVideoQualityLabels());

            return new VideoDetails() { id = videoID, ChannelName = video.Author, Title = video.Title, qualities = qualities, thumbnails = video.Thumbnails };
        }

        /// <summary>
        /// Downloads video and saves it to the provided videoPath folder
        /// </summary>
        /// <param name="id">ID of the video to download</param>
        /// <param name="quality">Choosen quality of the video</param>
        /// <param name="videoPath">Path where file should be saved and what file should be named with extension at the end (.mp4/.mp3).</param>
        /// <returns></returns>
        public async Task DownloadVideo(string id, string quality, string videoPath)
        {
            MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
            var audioStreamInfo = streamInfoSet.Audio.WithHighestBitrate();
            var videoStreamInfo = streamInfoSet.Video.FirstOrDefault(c => c.VideoQualityLabel == quality);
            var mediaStreamInfos = new MediaStreamInfo[] { audioStreamInfo, videoStreamInfo };
            await converter.DownloadAndProcessMediaStreamsAsync(mediaStreamInfos, videoPath, "mp4");
        }

        /// <summary>
        ///  Downloads audio and saves it to the provided audioPath folder
        /// </summary>
        /// <param name="id">id of the youtube video</param>
        /// <param name="audioPath">Path where file should be saved without extension at the end</param>
        /// <returns></returns>
        public async Task DownloadAudio(string id, string audioPath)
        {
            MediaStreamInfoSet streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
            var audioInfo = streamInfoSet.Audio.WithHighestBitrate();
            await client.DownloadMediaStreamAsync(audioInfo, audioPath);
        }

         IEnumerable<string> SortQualities(IEnumerable<string> qualities)
        {
            return qualities.ToList()
                    .Select(s => new { str = s, split = s.Split('p') })
                    .OrderBy(x => int.Parse(x.split[0]))
                    .ThenBy(x => x.split[1])
                    .Select(x => x.str)
                    .ToList();
        }
    }
}
