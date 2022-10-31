import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerContactCreateComponent } from './customer-contact-create.component';

describe('CustomerContactCreateComponent', () => {
  let component: CustomerContactCreateComponent;
  let fixture: ComponentFixture<CustomerContactCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerContactCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerContactCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
