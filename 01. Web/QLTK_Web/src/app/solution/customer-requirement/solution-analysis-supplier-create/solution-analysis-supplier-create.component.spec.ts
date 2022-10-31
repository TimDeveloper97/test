import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionAnalysisSupplierCreateComponent } from './solution-analysis-supplier-create.component';

describe('SolutionAnalysisSupplierCreateComponent', () => {
  let component: SolutionAnalysisSupplierCreateComponent;
  let fixture: ComponentFixture<SolutionAnalysisSupplierCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolutionAnalysisSupplierCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionAnalysisSupplierCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
