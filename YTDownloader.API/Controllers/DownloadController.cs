using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YTDownloader.API.Models;
using Microsoft.AspNetCore.Hosting;
using YTDownloader.API.Domain.Abstract;
using YTDownloader.API.Custom;
using Microsoft.AspNetCore.Authorization;
using YTDownloader.EFDataAccess.Models;
using System.Collections.Generic;

namespace YTDownloader.API.Controllers
{
    //http://localhost:63219/
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IYoutubeClientHelper clientHelper;
        private readonly IWebHostEnvironment env;
        private readonly IAccountPermissionChecker checker;

        public DownloadController(IYoutubeClientHelper client, IWebHostEnvironment env, IAccountPermissionChecker checker)
        {
            this.clientHelper = client;
            this.env = env;
            this.checker = checker;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetVideoMetaData(string videoUrl)
        {

            VideoDetails details;
            try
            {
                details = await clientHelper.GetVideoMetadata(clientHelper.GetVideoID(videoUrl));
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
            //Check if provided quality as parameter is available in the video
            var videoMetadata = await clientHelper.GetVideoMetadata(id);
            if (!((List<string>)videoMetadata.qualities).Contains(quality))
                return BadRequest("Invalid quality !");

            //Check if user is authorized and has required AccountLevel
            var isAlowedToDownload = await checker.CanDownloadInCertainQuality(quality, User.Identity.IsAuthenticated, User.Identity.Name);
            if (!isAlowedToDownload.isAllowed)
                return BadRequest(isAlowedToDownload.errorMessageIfNotAllowed);


            string videoPath = env.WebRootPath + $"\\DownloadedVideos\\{id}.mp4";
            try
            {
                await clientHelper.DownloadVideo(id, quality, videoPath);
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

            string audioPath = env.WebRootPath + $"\\DownloadedVideos\\{id}.mp3";

            try
            {
                await clientHelper.DownloadAudio(id, audioPath);
            }
            catch (ArgumentException)
            {
                return BadRequest(new string("Provided ID is incorrect"));
            }

            return new PhysicalFileResultAndDelete(audioPath, "audio/mp3");
        }
    }
}
