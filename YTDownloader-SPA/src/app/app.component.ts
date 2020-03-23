import { Component, OnInit } from '@angular/core';
import {AuthService} from './services/Auth/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Youtube Downloader';

constructor(public authService: AuthService) {
}

jwtHelper = new JwtHelperService();

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
        this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}
