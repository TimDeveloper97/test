import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPhaseManageComponent } from './project-phase-manage.component';

describe('ProjectPhaseManageComponent', () => {
  let component: ProjectPhaseManageComponent;
  let fixture: ComponentFixture<ProjectPhaseManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectPhaseManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPhaseManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
