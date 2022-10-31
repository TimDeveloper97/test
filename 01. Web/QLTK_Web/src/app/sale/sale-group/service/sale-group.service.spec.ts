import { TestBed } from '@angular/core/testing';

import { SaleGroupService } from './sale-group.service';

describe('SaleGroupService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SaleGroupService = TestBed.get(SaleGroupService);
    expect(service).toBeTruthy();
  });
});
