import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCompareSourceManageComponent } from './product-compare-source-manage.component';

describe('ProductCompareSourceManageComponent', () => {
  let component: ProductCompareSourceManageComponent;
  let fixture: ComponentFixture<ProductCompareSourceManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductCompareSourceManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductCompareSourceManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
