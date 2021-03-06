import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { VideoMetadataComponent } from './videoDownload/videoDownload.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './services/Auth/auth.service';
import { FileDownloadService } from './services/FileDownload/FileDownload.service';
import { RegisterFormComponent } from './register-form/register-form.component';
import {ToastrModule, ToastrService} from 'ngx-toastr';
import { YTGoldComponent } from './YTGold/YTGold.component';


@NgModule({
   declarations: [
      AppComponent,
      VideoMetadataComponent,
      NavComponent,
      RegisterFormComponent,
      YTGoldComponent
   ],
   imports: [
      BrowserModule,
      NgbModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      ToastrModule.forRoot({
         positionClass: 'toast-bottom-right',
         progressBar: true,
      })
   ],
   providers: [
      AuthService,
      FileDownloadService
   ],
   bootstrap: [
      AppComponent
   ],

   entryComponents: [
      RegisterFormComponent,
      YTGoldComponent
    ]
})
export class AppModule { }
