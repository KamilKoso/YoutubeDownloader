import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule} from '@angular/forms';

import { AppComponent } from './app.component';
import { VideoMetadataComponent } from './videoMetadata/videoMetadata.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';



@NgModule({
   declarations: [
      AppComponent,
      VideoMetadataComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
