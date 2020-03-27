using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YTDownloader.API.Models
{
    public class DownloaderConfiguration
    {
         public static string ffmpegLocation { get; private set; }
         public static string videoDownloadPath { get; private set; }
        public static string ffmpegDownloadLink { get; private set; }

        public static void SetSettings(string _ffmpegLocation, string _videoDownloadPath, string _ffmpegDownloadLink)
        {
            ffmpegLocation = _ffmpegLocation;
            videoDownloadPath = _videoDownloadPath;
            ffmpegDownloadLink = _ffmpegDownloadLink;
        }
    }
}
