import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectAttachTabTypeComponent } from './project-attach-tab-type.component';

describe('ProjectAttachTabTypeComponent', () => {
  let component: ProjectAttachTabTypeComponent;
  let fixture: ComponentFixture<ProjectAttachTabTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectAttachTabTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectAttachTabTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
