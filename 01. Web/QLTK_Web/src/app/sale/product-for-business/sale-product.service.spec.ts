import { TestBed } from '@angular/core/testing';

import { SaleProductService } from './sale-product.service';

describe('SaleProductService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SaleProductService = TestBed.get(SaleProductService);
    expect(service).toBeTruthy();
  });
});
