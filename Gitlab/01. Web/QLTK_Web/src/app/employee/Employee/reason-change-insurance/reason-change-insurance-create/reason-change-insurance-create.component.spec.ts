import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasonChangeInsuranceCreateComponent } from './reason-change-insurance-create.component';

describe('ReasonChangeInsuranceCreateComponent', () => {
  let component: ReasonChangeInsuranceCreateComponent;
  let fixture: ComponentFixture<ReasonChangeInsuranceCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReasonChangeInsuranceCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasonChangeInsuranceCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
