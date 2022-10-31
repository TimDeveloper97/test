import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerQuotationComponent } from './customer-quotation.component';

describe('CustomerQuotationComponent', () => {
  let component: CustomerQuotationComponent;
  let fixture: ComponentFixture<CustomerQuotationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerQuotationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerQuotationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
