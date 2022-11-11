import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectGeneralDesignComponent } from './project-general-design.component';

describe('ProjectGeneralDesignComponent', () => {
  let component: ProjectGeneralDesignComponent;
  let fixture: ComponentFixture<ProjectGeneralDesignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectGeneralDesignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectGeneralDesignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
