import { Component, OnInit } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';

import {FileDownloadService} from '../services/FileDownload/FileDownload.service';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'app-video-metadata',
  templateUrl: './VideoMetadata.component.html',
  styleUrls: ['./VideoMetadata.component.css']
})
export class VideoMetadataComponent implements OnInit {

  baseURL = 'http://localhost:5000';
  videoMetadata: any;
  videoUrl: string;
  selectedQuality: string;

  constructor(private http: HttpClient, private fileService: FileDownloadService) { }

  ngOnInit() { }

  onQualitySubmit() {
    console.log(this.selectedQuality);
  }

  downloadFile() {
    this.fileService.downloadFile(this.selectedQuality, this.videoMetadata.id).subscribe(response => {
      const blob: any = new Blob([response.body], {type: 'application/x-www-form-urlencoded'});
      fileSaver.saveAs(blob, this.videoMetadata.title + '.mp4');
    });
  }

  getMetadata() {
    let params = new HttpParams();
    params = params.append('videoUrl', this.videoUrl);
    this.http.get(this.baseURL + '/VideoDownload/GetVideoMetaData', {params})
    .subscribe(response => {
        this.videoMetadata = response;
    },
    error => {
      console.log(error);
    });
  }

}
