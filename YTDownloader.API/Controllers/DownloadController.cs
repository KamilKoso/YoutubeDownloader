using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YTDownloader.API.Models;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Custom;
using System.Collections.Generic;

namespace YTDownloader.API.Controllers
{
    //http://localhost:63219/
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IYoutubeClientHelper clientHelper;
        private readonly IAccountPermissionChecker checker;

        public DownloadController(IYoutubeClientHelper client, IAccountPermissionChecker checker)
        {
            clientHelper = client;
            this.checker = checker;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetVideoMetaData(string videoUrl)
        {

            VideoDetails details;
            try
            {
                details = await clientHelper.GetVideoMetadataAsync(clientHelper.GetVideoID(videoUrl));
            }
            catch (FormatException)
            {
                return BadRequest("Provided URL is incorrect");
            }
            return Ok(details);
        }


        //Saves video in the wwwroot/DownloadedVideo dir and returns it .
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetVideo(string id, string quality)
        {  
             string videoPath = DownloaderConfiguration.videoDownloadPath + $"\\{id}.mp4";
             
            //Check if provided quality as parameter is available in the video
            var videoMetadata = await clientHelper.GetVideoMetadataAsync(id);
            if (!((List<string>)videoMetadata.qualities).Contains(quality))
                return BadRequest("Invalid quality !");

            //Check if user is authorized and has required AccountLevel
            var isAlowedToDownload = await checker.CanDownloadInCertainQualityAsync(quality, User.Identity.IsAuthenticated, User.Identity.Name);
            if (!isAlowedToDownload.isAllowed)
                return BadRequest(isAlowedToDownload.errorMessageIfNotAllowed);

            try
            {
                await clientHelper.DownloadVideoAsync(id, quality, videoPath);
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            return new PhysicalFileResultAndDelete(videoPath, "video/mp4");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetAudio(string id)
        {

            string audioPath = DownloaderConfiguration.videoDownloadPath + $"\\DownloadedVideos\\{id}.mp3";

            try
            {
                await clientHelper.DownloadAudioAsync(id, audioPath);
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            return new PhysicalFileResultAndDelete(audioPath, "audio/mp3");
        }
    }
}
