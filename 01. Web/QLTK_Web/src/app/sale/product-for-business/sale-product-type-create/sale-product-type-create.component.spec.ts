import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleProductTypeCreateComponent } from './sale-product-type-create.component';

describe('SaleProductTypeCreateComponent', () => {
  let component: SaleProductTypeCreateComponent;
  let fixture: ComponentFixture<SaleProductTypeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleProductTypeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleProductTypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
