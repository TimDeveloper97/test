import { TestBed } from '@angular/core/testing';

import { GuidelineService } from './guideline.service';

describe('GuidelineService', () => {
  let service: GuidelineService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GuidelineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
