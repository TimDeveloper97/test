import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingJoinFinishComponent } from './meeting-join-finish.component';

describe('MeetingJoinFinishComponent', () => {
  let component: MeetingJoinFinishComponent;
  let fixture: ComponentFixture<MeetingJoinFinishComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MeetingJoinFinishComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetingJoinFinishComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
