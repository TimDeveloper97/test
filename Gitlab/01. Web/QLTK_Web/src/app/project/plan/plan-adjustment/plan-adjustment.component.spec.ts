import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanAdjustmentComponent } from './plan-adjustment.component';

describe('PlanAdjustmentComponent', () => {
  let component: PlanAdjustmentComponent;
  let fixture: ComponentFixture<PlanAdjustmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanAdjustmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanAdjustmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
