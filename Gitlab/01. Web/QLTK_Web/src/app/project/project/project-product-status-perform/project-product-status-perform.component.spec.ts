import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectProductStatusPerformComponent } from './project-product-status-perform.component';

describe('ProjectProductStatusPerformComponent', () => {
  let component: ProjectProductStatusPerformComponent;
  let fixture: ComponentFixture<ProjectProductStatusPerformComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectProductStatusPerformComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectProductStatusPerformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
