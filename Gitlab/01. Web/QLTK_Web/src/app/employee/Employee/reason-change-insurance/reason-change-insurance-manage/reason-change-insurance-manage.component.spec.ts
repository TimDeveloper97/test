import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasonChangeInsuranceManageComponent } from './reason-change-insurance-manage.component';

describe('ReasonChangeInsuranceManageComponent', () => {
  let component: ReasonChangeInsuranceManageComponent;
  let fixture: ComponentFixture<ReasonChangeInsuranceManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReasonChangeInsuranceManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasonChangeInsuranceManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
