import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCompareSourcModuleDetailComponent } from './product-compare-sourc-module-detail.component';

describe('ProductCompareSourcModuleDetailComponent', () => {
  let component: ProductCompareSourcModuleDetailComponent;
  let fixture: ComponentFixture<ProductCompareSourcModuleDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductCompareSourcModuleDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductCompareSourcModuleDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
