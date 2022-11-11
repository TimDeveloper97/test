import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskTimeStandardChooseYearModalComponent } from './task-time-standard-choose-year-modal.component';

describe('TaskTimeStandardChooseYearModalComponent', () => {
  let component: TaskTimeStandardChooseYearModalComponent;
  let fixture: ComponentFixture<TaskTimeStandardChooseYearModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskTimeStandardChooseYearModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskTimeStandardChooseYearModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
