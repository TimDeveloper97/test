import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskTimeStandardManageComponent } from './task-time-standard-manage.component';

describe('TaskTimeStandardManageComponent', () => {
  let component: TaskTimeStandardManageComponent;
  let fixture: ComponentFixture<TaskTimeStandardManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskTimeStandardManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskTimeStandardManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
