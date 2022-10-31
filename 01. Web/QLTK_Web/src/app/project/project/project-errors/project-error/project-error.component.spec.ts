import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectErrorComponent } from './project-error.component';

describe('ProjectErrorComponent', () => {
  let component: ProjectErrorComponent;
  let fixture: ComponentFixture<ProjectErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
