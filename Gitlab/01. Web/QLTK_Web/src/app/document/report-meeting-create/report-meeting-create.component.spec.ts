import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportMeetingCreateComponent } from './report-meeting-create.component';

describe('ReportMeetingCreateComponent', () => {
  let component: ReportMeetingCreateComponent;
  let fixture: ComponentFixture<ReportMeetingCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportMeetingCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportMeetingCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
