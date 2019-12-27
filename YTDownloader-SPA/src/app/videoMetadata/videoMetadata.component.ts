import { Component, OnInit } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-video-metadata',
  templateUrl: './VideoMetadata.component.html',
  styleUrls: ['./VideoMetadata.component.css']
})
export class VideoMetadataComponent implements OnInit {

  videoMetadata: any;
  videoUrl: string;
  selectedQuality: string;

  constructor(private http: HttpClient) { }

  ngOnInit() { }

  onQualitySubmit() {
    console.log(this.selectedQuality);
  }

  getMetadata() {
    let params = new HttpParams();
    params = params.append('videoUrl', this.videoUrl);
    this.http.get('http://localhost:5000/VideoDownload/GetVideoMetaData', {params})
    .subscribe(response => {
        this.videoMetadata = response;
    },
    error => {
      console.log(error);
    });
  }

}
