import { Injectable } from '@angular/core';
import {HttpResponse, HttpHeaders, HttpClient, HttpParams} from '@angular/common/http';
import {ResponseContentType, ResponseType} from '@angular/http';
import {Observable} from 'rxjs';
import * as globals from '../../global';

@Injectable({
  providedIn: 'root'
})
export class FileDownloadService {


constructor(private http: HttpClient) {}

 downloadVideo(quality, videoID): Observable<HttpResponse<Blob>> {
  let params = new HttpParams();
  params = params.append('quality', quality).append('id', videoID);

  return this.http.post(globals.baseApiUrl + '/Download/GetVideo', undefined , {observe: 'response', responseType: 'blob', params} );
  }

  downloadAudio(videoID): Observable<HttpResponse<Blob>> {
    let params = new HttpParams();
    params = params.append('id', videoID);
    return this.http.post(globals.baseApiUrl + '/Download/GetAudio', undefined , {observe: 'response', responseType: 'blob', params} );
    }
}

