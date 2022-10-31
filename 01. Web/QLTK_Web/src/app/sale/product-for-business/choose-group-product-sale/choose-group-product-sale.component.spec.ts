import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseGroupProductSaleComponent } from './choose-group-product-sale.component';

describe('ChooseGroupProductSaleComponent', () => {
  let component: ChooseGroupProductSaleComponent;
  let fixture: ComponentFixture<ChooseGroupProductSaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseGroupProductSaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseGroupProductSaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
