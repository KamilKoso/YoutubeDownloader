import { Component, OnInit } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-VideoMetadata',
  templateUrl: './VideoMetadata.component.html',
  styleUrls: ['./VideoMetadata.component.css']
})
export class VideoMetadataComponent implements OnInit {

  videoMetadata: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getMetadata('xd'); 
  }
  //Jak narazie wywolywana jest metoda bez parametrów, parametr videoURL jest podany wprosts
  getMetadata(url: string) {
    let params = new HttpParams();
    params = params.append('videoUrl',url);
    this.http.get('http://localhost:5000/VideoDownload/GetVideoMetaData?videoUrl=https://www.youtube.com/watch?v=4ZIoHtGgIkA')
    .subscribe(response => {
        this.videoMetadata = response;
    },
    error => {
      console.log(error)
    });
  }

}
