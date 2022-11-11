import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectEmployeeCreateComponent } from './project-employee-create.component';

describe('ProjectEmployeeCreateComponent', () => {
  let component: ProjectEmployeeCreateComponent;
  let fixture: ComponentFixture<ProjectEmployeeCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectEmployeeCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectEmployeeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
