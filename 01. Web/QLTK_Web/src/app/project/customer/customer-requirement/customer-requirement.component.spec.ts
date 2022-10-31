import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerRequirementComponent } from './customer-requirement.component';

describe('CustomerRequirementComponent', () => {
  let component: CustomerRequirementComponent;
  let fixture: ComponentFixture<CustomerRequirementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerRequirementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerRequirementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
