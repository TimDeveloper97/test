import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardManageComponent } from './product-standard-manage.component';

describe('ProductStandardManageComponent', () => {
  let component: ProductStandardManageComponent;
  let fixture: ComponentFixture<ProductStandardManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
