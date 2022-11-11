import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerRequirequirmentNeedToHandleComponent } from './customer-requirequirment-need-to-handle.component';

describe('CustomerRequirequirmentNeedToHandleComponent', () => {
  let component: CustomerRequirequirmentNeedToHandleComponent;
  let fixture: ComponentFixture<CustomerRequirequirmentNeedToHandleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerRequirequirmentNeedToHandleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerRequirequirmentNeedToHandleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
