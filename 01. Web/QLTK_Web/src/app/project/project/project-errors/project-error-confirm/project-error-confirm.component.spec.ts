import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectErrorConfirmComponent } from './project-error-confirm.component';

describe('ProjectErrorConfirmComponent', () => {
  let component: ProjectErrorConfirmComponent;
  let fixture: ComponentFixture<ProjectErrorConfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectErrorConfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectErrorConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
