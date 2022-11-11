import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectErrorHistoryChangePlanComponent } from './project-error-history-change-plan.component';

describe('ProjectErrorHistoryChangePlanComponent', () => {
  let component: ProjectErrorHistoryChangePlanComponent;
  let fixture: ComponentFixture<ProjectErrorHistoryChangePlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectErrorHistoryChangePlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectErrorHistoryChangePlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
