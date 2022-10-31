import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseQuotationStepComponent } from './show-choose-quotation-step.component';

describe('ShowChooseQuotationStepComponent', () => {
  let component: ShowChooseQuotationStepComponent;
  let fixture: ComponentFixture<ShowChooseQuotationStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowChooseQuotationStepComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseQuotationStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
