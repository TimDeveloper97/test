import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectGeneralDesignManagaComponent } from './project-general-design-managa.component';

describe('ProjectGeneralDesignManagaComponent', () => {
  let component: ProjectGeneralDesignManagaComponent;
  let fixture: ComponentFixture<ProjectGeneralDesignManagaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectGeneralDesignManagaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectGeneralDesignManagaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
