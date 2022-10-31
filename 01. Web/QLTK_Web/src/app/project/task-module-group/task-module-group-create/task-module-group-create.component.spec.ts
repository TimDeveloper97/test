import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskModuleGroupCreateComponent } from './task-module-group-create.component';

describe('TaskModuleGroupCreateComponent', () => {
  let component: TaskModuleGroupCreateComponent;
  let fixture: ComponentFixture<TaskModuleGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskModuleGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskModuleGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
