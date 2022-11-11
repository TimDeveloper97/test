import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorktypeInterviewCreateComponent } from './worktype-interview-create.component';

describe('WorktypeInterviewCreateComponent', () => {
  let component: WorktypeInterviewCreateComponent;
  let fixture: ComponentFixture<WorktypeInterviewCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorktypeInterviewCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorktypeInterviewCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
