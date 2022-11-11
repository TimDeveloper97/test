import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardTpaManageComponent } from './product-standard-tpa-manage.component';

describe('ProductStandardTpaManageComponent', () => {
  let component: ProductStandardTpaManageComponent;
  let fixture: ComponentFixture<ProductStandardTpaManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardTpaManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardTpaManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
