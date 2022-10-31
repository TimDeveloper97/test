import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseCustomerContactComponent } from './choose-customer-contact.component';

describe('ChooseCustomerContactComponent', () => {
  let component: ChooseCustomerContactComponent;
  let fixture: ComponentFixture<ChooseCustomerContactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseCustomerContactComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseCustomerContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
