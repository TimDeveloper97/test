import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCompareSourcDeviceDetailComponent } from './product-compare-sourc-device-detail.component';

describe('ProductCompareSourcDeviceDetailComponent', () => {
  let component: ProductCompareSourcDeviceDetailComponent;
  let fixture: ComponentFixture<ProductCompareSourcDeviceDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductCompareSourcDeviceDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductCompareSourcDeviceDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
