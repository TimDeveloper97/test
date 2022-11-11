import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectProductMaterialCompareComponent } from './project-product-material-compare.component';

describe('ProjectProductMaterialCompareComponent', () => {
  let component: ProjectProductMaterialCompareComponent;
  let fixture: ComponentFixture<ProjectProductMaterialCompareComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectProductMaterialCompareComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectProductMaterialCompareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
