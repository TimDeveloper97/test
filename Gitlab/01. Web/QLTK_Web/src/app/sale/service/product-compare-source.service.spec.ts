import { TestBed } from '@angular/core/testing';

import { ProductCompareSourceService } from './product-compare-source.service';

describe('ProductCompareSourceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProductCompareSourceService = TestBed.get(ProductCompareSourceService);
    expect(service).toBeTruthy();
  });
});
