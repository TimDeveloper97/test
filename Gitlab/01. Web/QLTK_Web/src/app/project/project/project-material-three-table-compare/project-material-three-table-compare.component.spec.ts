import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectMaterialThreeTableCompareComponent } from './project-material-three-table-compare.component';

describe('ProjectMaterialThreeTableCompareComponent', () => {
  let component: ProjectMaterialThreeTableCompareComponent;
  let fixture: ComponentFixture<ProjectMaterialThreeTableCompareComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectMaterialThreeTableCompareComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectMaterialThreeTableCompareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
