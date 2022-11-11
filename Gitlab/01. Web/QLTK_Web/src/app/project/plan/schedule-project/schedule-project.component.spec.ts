import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleProjectComponent } from './schedule-project.component';

describe('ScheduleProjectComponent', () => {
  let component: ScheduleProjectComponent;
  let fixture: ComponentFixture<ScheduleProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
