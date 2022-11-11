import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectEmployeeUpdateStatusSubsidyHistoryComponent } from './project-employee-update-status-subsidy-history.component';

describe('ProjectEmployeeUpdateStatusSubsidyHistoryComponent', () => {
  let component: ProjectEmployeeUpdateStatusSubsidyHistoryComponent;
  let fixture: ComponentFixture<ProjectEmployeeUpdateStatusSubsidyHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectEmployeeUpdateStatusSubsidyHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectEmployeeUpdateStatusSubsidyHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
