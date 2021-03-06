import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import * as globals from '../../global';
import { map } from 'rxjs/operators';
import {JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

constructor(private http: HttpClient) { }

  jwtHelper = new JwtHelperService();
  decodedToken: any;

login(model: any) {
  return this.http.post(globals.baseApiUrl + '/auth/login', model)
        .pipe(map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
          }
        }));
}

register(model: any) {return this.http.post(globals.baseApiUrl + '/auth/register', model); }

loggedIn() {
 const token = localStorage.getItem('token');
 return !this.jwtHelper.isTokenExpired(token);
}

getToken() {
  return localStorage.getItem('token');
  }

changeAccountLevel(accountLevel: any){
  let params = new HttpParams();
  params = params.append('username', this.decodedToken.unique_name).append('level', accountLevel);

  return this.http.post(globals.baseApiUrl + '/auth/ChangeAccountLevel', undefined,
  {
    params,
    responseType: 'text',
    headers: {Authorization: `Bearer ${this.getToken()}`}
  });
}
}
