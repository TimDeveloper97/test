import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPhaseCreateComponent } from './project-phase-create.component';

describe('ProjectPhaseCreateComponent', () => {
  let component: ProjectPhaseCreateComponent;
  let fixture: ComponentFixture<ProjectPhaseCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectPhaseCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPhaseCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
