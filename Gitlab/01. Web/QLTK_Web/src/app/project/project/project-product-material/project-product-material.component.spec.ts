import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectProductMaterialComponent } from './project-product-material.component';

describe('ProjectProductMaterialComponent', () => {
  let component: ProjectProductMaterialComponent;
  let fixture: ComponentFixture<ProjectProductMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectProductMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectProductMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
