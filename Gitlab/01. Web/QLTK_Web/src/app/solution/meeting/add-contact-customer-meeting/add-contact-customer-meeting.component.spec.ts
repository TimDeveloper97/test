import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddContactCustomerMeetingComponent } from './add-contact-customer-meeting.component';

describe('AddContactCustomerMeetingComponent', () => {
  let component: AddContactCustomerMeetingComponent;
  let fixture: ComponentFixture<AddContactCustomerMeetingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddContactCustomerMeetingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddContactCustomerMeetingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
