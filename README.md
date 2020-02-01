<h1 align="center">YoutubeDownloader</h1>

<h2>Introduction</h2>
This project is simple website that will download Mp3 or Mp4 files in every possibble quality from YouTube. It uses <a href="https://github.com/Tyrrrz/YoutubeExplode">YoutubeExplode</a> along with <a href="https://github.com/Tyrrrz/YoutubeExplode.Converter">YoutubeExplode.Converter</a> under the hood. 
<h3>Front-end</h3>
Front end of the app was made using Angular 8.2.14, Bootstrap 4.41, Animate.css 3.7.2. To be continued ...



<h2>Back-end</h2>
When server is starting, it deletes all the files and directories that are in /wwwroot/DownloadedVideos. If your server crashes when videos/audios were being processed this will automatically get rid of garbage left in this dir. When server is up and running, it can process videos. To do it properly it needs to know location of ffmpeg.exe, you can set it when creating instance of YoutubeClientHelper. By default it is coded to be in /wwwroot dir.

**API's**

**1. GetVideoMetaData** 
localhost:{port}/Download/GetVideoMetaData?videoUrl={LinkToYoutube}
This api collects and returns metadata of videoUrl. Saves it to VideoDetails model. VideoDetails model contains: ID of the Youtube video, Channel name, Title of the Youtube video, Qualities of the Youtube video, Thumbnails in different resolutions

**2. GetVideo**
localhost:{port}/Download/GetVideo?id={YoutubeVideoID}&quality={VideoQualityToDownload}
This api downloads Youtube video based on it's id. It stores video and audio stream in /wwwroot/DownloadedVideos, ffmpeg converts it to .mp4 file then it is being loaded to the MemoryStream, file is being deleted from disk, and from MemoryStream it is being returned.

**3. GetAudio**
localhost:{port}/Download/GetAudio?id={YoutubeVideoID}
This api lets you download .mp3 file in highest possible bitrate. It gets youtube ID, downloads it and stores in /wwwroot/DownloadedVideos (this directory is just temporarily, in the future it will be stored in /DownloadedAudios directory), then .mp3 file is being loaded to the MemoryStream, it is being deleted from the disk, and file is being returned from MemoryStream.

**Parameters:**
{LinkToYoutube} - Just a link to the youtube video. It doesen't work with playlists and not public videos. ex: https://www.youtube.com/watch?v=dQw4w9WgXcQ .
{YoutubeVideoID} - It is extracted id from youtube video. You can extract id by using GetVideoMetaData API. Youtube video id is string after "v=" in youtube link. Ex: https://www.youtube.com/watch?v=dQw4w9WgXcQ => ID="dQw4w9WgXcQ"
{VideoQualityToDownload} - It is quality in which video will be downloaded. Video needs to contain provided quality, we can't download video in 4k when video the best quality is 1080p.

<h2>How does it work ? </h2>
<img src="https://media.giphy.com/media/U5J6siZVheaK47wj53/giphy.gif" /><img src="https://media.giphy.com/media/LrLJaJTGvFB3qZ2wx8/giphy.gif" />
