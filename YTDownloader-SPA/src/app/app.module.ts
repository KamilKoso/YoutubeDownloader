import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule} from '@angular/forms';

import { AppComponent } from './app.component';
import { VideoMetadataComponent } from './videoMetadata/videoMetadata.component';



@NgModule({
   declarations: [
      AppComponent,
      VideoMetadataComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
