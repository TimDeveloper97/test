import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskTimeStandardCreateComponent } from './task-time-standard-create.component';

describe('TaskTimeStandardCreateComponent', () => {
  let component: TaskTimeStandardCreateComponent;
  let fixture: ComponentFixture<TaskTimeStandardCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskTimeStandardCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskTimeStandardCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
