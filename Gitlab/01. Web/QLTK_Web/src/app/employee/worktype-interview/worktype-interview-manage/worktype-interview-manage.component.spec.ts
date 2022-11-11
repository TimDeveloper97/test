import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorktypeInterviewManageComponent } from './worktype-interview-manage.component';

describe('WorktypeInterviewManageComponent', () => {
  let component: WorktypeInterviewManageComponent;
  let fixture: ComponentFixture<WorktypeInterviewManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorktypeInterviewManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorktypeInterviewManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
