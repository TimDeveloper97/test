import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectErrorCreateComponent } from './project-error-create.component';

describe('ProjectErrorCreateComponent', () => {
  let component: ProjectErrorCreateComponent;
  let fixture: ComponentFixture<ProjectErrorCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectErrorCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectErrorCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
