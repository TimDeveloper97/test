import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerMeetingComponent } from './customer-meeting.component';

describe('CustomerMeetingComponent', () => {
  let component: CustomerMeetingComponent;
  let fixture: ComponentFixture<CustomerMeetingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerMeetingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerMeetingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
