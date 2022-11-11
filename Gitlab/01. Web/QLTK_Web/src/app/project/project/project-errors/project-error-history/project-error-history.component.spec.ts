import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectErrorHistoryComponent } from './project-error-history.component';

describe('ProjectErrorHistoryComponent', () => {
  let component: ProjectErrorHistoryComponent;
  let fixture: ComponentFixture<ProjectErrorHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectErrorHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectErrorHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
