import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportPlanByDateComponent } from './report-plan-by-date.component';

describe('ReportPlanByDateComponent', () => {
  let component: ReportPlanByDateComponent;
  let fixture: ComponentFixture<ReportPlanByDateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportPlanByDateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportPlanByDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
