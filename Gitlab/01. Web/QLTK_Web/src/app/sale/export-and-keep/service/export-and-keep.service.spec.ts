import { TestBed } from '@angular/core/testing';

import { ExportAndKeepService } from './export-and-keep.service';

describe('ExportAndKeepService', () => {
  let service: ExportAndKeepService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExportAndKeepService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
