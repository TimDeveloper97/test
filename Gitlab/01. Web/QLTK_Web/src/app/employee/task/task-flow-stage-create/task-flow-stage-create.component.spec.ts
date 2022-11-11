import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskFlowStageCreateComponent } from './task-flow-stage-create.component';

describe('TaskFlowStageCreateComponent', () => {
  let component: TaskFlowStageCreateComponent;
  let fixture: ComponentFixture<TaskFlowStageCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TaskFlowStageCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskFlowStageCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
