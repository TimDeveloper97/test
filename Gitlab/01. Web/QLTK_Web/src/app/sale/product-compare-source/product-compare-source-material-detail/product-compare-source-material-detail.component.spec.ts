import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCompareSourceMaterialDetailComponent } from './product-compare-source-material-detail.component';

describe('ProductCompareSourceMaterialDetailComponent', () => {
  let component: ProductCompareSourceMaterialDetailComponent;
  let fixture: ComponentFixture<ProductCompareSourceMaterialDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductCompareSourceMaterialDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductCompareSourceMaterialDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
