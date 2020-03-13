/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { VideoMetadataComponent } from './videoDownload.component';

describe('VideoMetadataComponent', () => {
  let component: VideoMetadataComponent;
  let fixture: ComponentFixture<VideoMetadataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VideoMetadataComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideoMetadataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
