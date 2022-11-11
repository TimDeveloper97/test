import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerRequirementCreateComponent } from './customer-requirement-create.component';

describe('CustomerRequirementCreateComponent', () => {
  let component: CustomerRequirementCreateComponent;
  let fixture: ComponentFixture<CustomerRequirementCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerRequirementCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerRequirementCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});