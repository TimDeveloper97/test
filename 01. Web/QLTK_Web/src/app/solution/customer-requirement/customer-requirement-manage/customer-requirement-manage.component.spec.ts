import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerRequirementManageComponent } from './customer-requirement-manage.component';

describe('CustomerRequirementManageComponent', () => {
  let component: CustomerRequirementManageComponent;
  let fixture: ComponentFixture<CustomerRequirementManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerRequirementManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerRequirementManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
