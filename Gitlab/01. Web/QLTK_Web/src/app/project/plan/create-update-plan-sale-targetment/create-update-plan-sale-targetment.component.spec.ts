import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdatePlanSaleTargetmentComponent } from './create-update-plan-sale-targetment.component';

describe('CreateUpdatePlanSaleTargetmentComponent', () => {
  let component: CreateUpdatePlanSaleTargetmentComponent;
  let fixture: ComponentFixture<CreateUpdatePlanSaleTargetmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdatePlanSaleTargetmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdatePlanSaleTargetmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
