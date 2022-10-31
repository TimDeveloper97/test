import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskModuleGroupManageComponent } from './task-module-group-manage.component';

describe('TaskModuleGroupManageComponent', () => {
  let component: TaskModuleGroupManageComponent;
  let fixture: ComponentFixture<TaskModuleGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskModuleGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskModuleGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
