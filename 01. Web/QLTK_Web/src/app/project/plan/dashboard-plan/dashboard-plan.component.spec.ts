import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPlanComponent } from './dashboard-plan.component';

describe('DashboardPlanComponent', () => {
  let component: DashboardPlanComponent;
  let fixture: ComponentFixture<DashboardPlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardPlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
