import { Component, OnInit } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import {VideoDownloadStatus} from '../helpers/VideoDownloadStatusEnum';
import * as globals from '../global';


import {FileDownloadService} from '../services/FileDownload/FileDownload.service';
import * as fileSaver from 'file-saver';
import {fade, fadeFast} from '../services/Animations/fade';
import {ToastrService} from 'ngx-toastr';




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
  gettingMetadata = false;
  animationLeftOrRight: string;

  MediaTypes = ['Mp3', 'Mp4'];

  status: VideoDownloadStatus = VideoDownloadStatus.NotDownloading;

  constructor(private http: HttpClient, private fileService: FileDownloadService, private toastr: ToastrService) { }

  ngOnInit() { }


  downloadAudio() {
    this.toastr.info('Starting processing your video. This may take a while !');
    this.status = VideoDownloadStatus.DownloadingToTheServer;

    this.fileService.downloadAudio(this.videoMetadata.id).subscribe(response => {
      this.status = VideoDownloadStatus.DownloadingFromServer;
      const blob: any = new Blob([response.body], {type: 'application/x-www-form-urlencoded'});
      fileSaver.saveAs(blob, this.videoMetadata.title + '.mp3');
    },
    (error: HttpErrorResponse) => {
      this.status = VideoDownloadStatus.DownloadingError;
      this.toastr.error(error.error);
    },
    () => {
      this.status = VideoDownloadStatus.DownloadingComplete;
      this.toastr.success('Processing completed !');
    });

  }

  downloadVideo() {
    this.toastr.info('Starting processing your video. This may take a while !');
    this.status = VideoDownloadStatus.DownloadingToTheServer;
    this.status = VideoDownloadStatus.DownloadingToTheServer;

    this.fileService.downloadVideo(this.selectedQuality, this.videoMetadata.id).subscribe(response => {
      const blob: any = new Blob([response.body], {type: 'application/x-www-form-urlencoded'});
      fileSaver.saveAs(blob, this.videoMetadata.title + '.mp4');
    },
    (error: HttpErrorResponse) => {
      const reader = new FileReader();
      this.status = VideoDownloadStatus.DownloadingError;
      reader.readAsText(error.error);
      reader.addEventListener('loadend', () => {
       this.toastr.error(reader.result.toString());
     });
    },
    () => {
      this.status = VideoDownloadStatus.DownloadingComplete;
      this.toastr.success('Processing completed !');
    });
  }

  getMetadata() {
    this.status = VideoDownloadStatus.NotDownloading;
    this.toastr.info('Gathering information about video !');
    this.getMetadataFailed = false;
    this.gettingMetadata = true;
    this.videoMetadata = null;
    this.selectedQuality = null;
    this.selectedMediaType = null;

    let params = new HttpParams();
    params = params.append('videoUrl', this.videoUrl);
    this.http.get(globals.baseApiUrl + '/Download/GetVideoMetaData', {params})
    .subscribe(response => {
        this.videoMetadata = response;
        this.gettingMetadata = false;
    },
    (error: HttpErrorResponse) => {
      this.getMetadataFailed = true;

      this.toastr.error('Error occured ! Try again');
      this.gettingMetadata = false;

    });
  }
}
