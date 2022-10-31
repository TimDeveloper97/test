import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectAttachCreateComponent } from './project-attach-create.component';

describe('ProjectAttachCreateComponent', () => {
  let component: ProjectAttachCreateComponent;
  let fixture: ComponentFixture<ProjectAttachCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectAttachCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectAttachCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
