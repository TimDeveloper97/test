import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectProductQcComponent } from './project-product-qc.component';

describe('ProjectProductQcComponent', () => {
  let component: ProjectProductQcComponent;
  let fixture: ComponentFixture<ProjectProductQcComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectProductQcComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectProductQcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
