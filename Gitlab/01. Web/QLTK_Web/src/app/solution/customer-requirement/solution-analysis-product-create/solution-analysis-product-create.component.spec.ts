import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionAnalysisProductCreateComponent } from './solution-analysis-product-create.component';

describe('SolutionAnalysisProductCreateComponent', () => {
  let component: SolutionAnalysisProductCreateComponent;
  let fixture: ComponentFixture<SolutionAnalysisProductCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolutionAnalysisProductCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionAnalysisProductCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
