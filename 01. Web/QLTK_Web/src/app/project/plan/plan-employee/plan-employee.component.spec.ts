import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanEmployeeComponent } from './plan-employee.component';

describe('PlanEmployeeComponent', () => {
  let component: PlanEmployeeComponent;
  let fixture: ComponentFixture<PlanEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanEmployeeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
