import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryQuotesPlanComponent } from './summary-quotes-plan.component';

describe('SummaryQuotesPlanComponent', () => {
  let component: SummaryQuotesPlanComponent;
  let fixture: ComponentFixture<SummaryQuotesPlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SummaryQuotesPlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryQuotesPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
