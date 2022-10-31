import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectRoleComponent } from './project-role.component';

describe('ProjectRoleComponent', () => {
  let component: ProjectRoleComponent;
  let fixture: ComponentFixture<ProjectRoleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectRoleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
