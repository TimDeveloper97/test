import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasonChangeIncomeCreateComponent } from './reason-change-income-create.component';

describe('ReasonChangeIncomeCreateComponent', () => {
  let component: ReasonChangeIncomeCreateComponent;
  let fixture: ComponentFixture<ReasonChangeIncomeCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReasonChangeIncomeCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasonChangeIncomeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
