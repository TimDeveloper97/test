import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductForBussinessComponent } from './product-for-bussiness.component';

describe('ProductForBussinessComponent', () => {
  let component: ProductForBussinessComponent;
  let fixture: ComponentFixture<ProductForBussinessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductForBussinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductForBussinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
