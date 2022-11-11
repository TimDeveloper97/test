import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskProblemProjectComponent } from './risk-problem-project.component';

describe('RiskProblemProjectComponent', () => {
  let component: RiskProblemProjectComponent;
  let fixture: ComponentFixture<RiskProblemProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RiskProblemProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskProblemProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
