import { Component, OnInit } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import {VideoDownloadStatus} from '../helpers/VideoDownloadStatusEnum';


import {FileDownloadService} from '../services/FileDownload/FileDownload.service';
import * as fileSaver from 'file-saver';
import {fade} from '../services/Animations/fade';




@Component({
  selector: 'app-video-metadata',
  templateUrl: './VideoMetadata.component.html',
  styleUrls: ['./VideoMetadata.component.css'],
  animations: [
    fade
  ]
})
export class VideoMetadataComponent implements OnInit {


  baseURL = 'http://localhost:5000';
  videoMetadata: any;
  videoUrl: string;
  selectedQuality: string;
  selectedMediaType = 'as';
  getMetadataFailed = false;

  MediaTypes = ['Mp3', 'Mp4'];

  status: VideoDownloadStatus = VideoDownloadStatus.NotDownloading;

  constructor(private http: HttpClient, private fileService: FileDownloadService) { }

  ngOnInit() { }

  downloadAudio() {

  }

  downloadVideo() {
    this.status = VideoDownloadStatus.DownloadingToTheServer;

    this.fileService.downloadFile(this.selectedQuality, this.videoMetadata.id).subscribe(response => {
      this.status = VideoDownloadStatus.DownloadingFromServer;
      const blob: any = new Blob([response.body], {type: 'application/x-www-form-urlencoded'});
      fileSaver.saveAs(blob, this.videoMetadata.title + '.mp4');
    },
    error => {
      this.status = VideoDownloadStatus.DownloadingError;
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

    let params = new HttpParams();
    params = params.append('videoUrl', this.videoUrl);
    this.http.get(this.baseURL + '/Download/GetVideoMetaData', {params})
    .subscribe(response => {
        this.videoMetadata = response;
    },
    error => {
      this.getMetadataFailed = true;
      console.log(error);
    });
  }

}
