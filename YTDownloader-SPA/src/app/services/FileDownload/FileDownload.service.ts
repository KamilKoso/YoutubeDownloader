import { Injectable } from '@angular/core';
import {HttpResponse, HttpHeaders, HttpClient, HttpParams} from '@angular/common/http';
import {ResponseContentType, ResponseType} from '@angular/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileDownloadService {

  baseURL = 'http://localhost:5000';

constructor(private http: HttpClient) {}

 downloadFile(quality, videoID): Observable<HttpResponse<Blob>> {
  let params = new HttpParams();
  params = params.append('quality', quality).append('id', videoID);

  return this.http.post(this.baseURL + '/Download/GetVideo', undefined , {observe: 'response', responseType: 'blob', params} );
  }
}

