/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FileDownloadService } from './FileDownload.service';

describe('Service: FileDownload', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FileDownloadService]
    });
  });

  it('should ...', inject([FileDownloadService], (service: FileDownloadService) => {
    expect(service).toBeTruthy();
  }));
});
