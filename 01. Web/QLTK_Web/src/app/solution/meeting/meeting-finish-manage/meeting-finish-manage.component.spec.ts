import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingFinishManageComponent } from './meeting-finish-manage.component';

describe('MeetingFinishManageComponent', () => {
  let component: MeetingFinishManageComponent;
  let fixture: ComponentFixture<MeetingFinishManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MeetingFinishManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetingFinishManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
