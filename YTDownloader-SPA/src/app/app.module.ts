import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { VideoMetadataComponent } from './videoDownload/videoDownload.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './services/Auth/auth.service';
import { FileDownloadService } from './services/FileDownload/FileDownload.service';


@NgModule({
   declarations: [
      AppComponent,
      VideoMetadataComponent,
      NavComponent
   ],
   imports: [
      BrowserModule,
      NgbModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
   ],
   providers: [
      AuthService,
      FileDownloadService,

   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
