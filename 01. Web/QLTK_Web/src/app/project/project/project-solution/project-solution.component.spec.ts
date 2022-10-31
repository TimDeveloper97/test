import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSolutionComponent } from './project-solution.component';

describe('ProjectSolutionComponent', () => {
  let component: ProjectSolutionComponent;
  let fixture: ComponentFixture<ProjectSolutionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectSolutionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSolutionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
