import { Component, OnInit } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import {VideoDownloadStatus} from '../helpers/VideoDownloadStatusEnum';
import * as globals from '../global';


import {FileDownloadService} from '../services/FileDownload/FileDownload.service';
import * as fileSaver from 'file-saver';
import {fade, fadeFast} from '../services/Animations/fade';




@Component({
  selector: 'app-video-metadata',
  templateUrl: './videoDownload.component.html',
  styleUrls: ['./videoDownload.component.css'],
  animations: [
    fade,
    fadeFast,
  ]
})
export class VideoMetadataComponent implements OnInit {


  videoMetadata: any;
  videoUrl: string;
  selectedQuality: string;
  selectedMediaType: string;
  getMetadataFailed = false;
  animationLeftOrRight: string;

  MediaTypes = ['Mp3', 'Mp4'];

  status: VideoDownloadStatus = VideoDownloadStatus.NotDownloading;

  constructor(private http: HttpClient, private fileService: FileDownloadService) { }

  ngOnInit() { }


  downloadAudio() {
    this.status = VideoDownloadStatus.DownloadingToTheServer;

    this.fileService.downloadAudio(this.videoMetadata.id).subscribe(response => {
      this.status = VideoDownloadStatus.DownloadingFromServer;
      const blob: any = new Blob([response.body], {type: 'application/x-www-form-urlencoded'});
      fileSaver.saveAs(blob, this.videoMetadata.title + '.mp3');
    },
    error => {
      this.status = VideoDownloadStatus.DownloadingError;
    },
    () => {
      this.status = VideoDownloadStatus.DownloadingComplete;
    });

  }

  downloadVideo() {
    this.status = VideoDownloadStatus.DownloadingToTheServer;

    this.fileService.downloadVideo(this.selectedQuality, this.videoMetadata.id).subscribe(response => {
      this.status = VideoDownloadStatus.DownloadingFromServer;
      const blob: any = new Blob([response.body], {type: 'application/x-www-form-urlencoded'});
      fileSaver.saveAs(blob, this.videoMetadata.title + '.mp4');
    },
    error => {
      this.status = VideoDownloadStatus.DownloadingError;
      console.log(error);
    },
    () => {
      this.status = VideoDownloadStatus.DownloadingComplete;
    });
  }

  getMetadata() {
    this.status = VideoDownloadStatus.NotDownloading;
    this.getMetadataFailed = false;
    this.videoMetadata = null;
    this.selectedQuality = null;
    this.selectedMediaType = null;

    let params = new HttpParams();
    params = params.append('videoUrl', this.videoUrl);
    this.http.get(globals.baseApiUrl + '/Download/GetVideoMetaData', {params})
    .subscribe(response => {
        this.videoMetadata = response;
    },
    error => {
      this.getMetadataFailed = true;
      console.log(error);
    });
  }
}
