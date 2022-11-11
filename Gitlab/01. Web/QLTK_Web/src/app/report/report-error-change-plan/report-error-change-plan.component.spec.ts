import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportErrorChangePlanComponent } from './report-error-change-plan.component';

describe('ReportErrorChangePlanComponent', () => {
  let component: ReportErrorChangePlanComponent;
  let fixture: ComponentFixture<ReportErrorChangePlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportErrorChangePlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportErrorChangePlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
