import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCompareSorceProductDetailComponent } from './product-compare-sorce-product-detail.component';

describe('ProductCompareSorceProductDetailComponent', () => {
  let component: ProductCompareSorceProductDetailComponent;
  let fixture: ComponentFixture<ProductCompareSorceProductDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductCompareSorceProductDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductCompareSorceProductDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
