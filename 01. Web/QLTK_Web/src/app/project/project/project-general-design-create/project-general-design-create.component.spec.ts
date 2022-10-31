import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectGeneralDesignCreateComponent } from './project-general-design-create.component';

describe('ProjectGeneralDesignCreateComponent', () => {
  let component: ProjectGeneralDesignCreateComponent;
  let fixture: ComponentFixture<ProjectGeneralDesignCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectGeneralDesignCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectGeneralDesignCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
