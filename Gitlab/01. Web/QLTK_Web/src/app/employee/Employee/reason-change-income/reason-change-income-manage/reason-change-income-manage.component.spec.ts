import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasonChangeIncomeManageComponent } from './reason-change-income-manage.component';

describe('ReasonChangeIncomeManageComponent', () => {
  let component: ReasonChangeIncomeManageComponent;
  let fixture: ComponentFixture<ReasonChangeIncomeManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReasonChangeIncomeManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasonChangeIncomeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
