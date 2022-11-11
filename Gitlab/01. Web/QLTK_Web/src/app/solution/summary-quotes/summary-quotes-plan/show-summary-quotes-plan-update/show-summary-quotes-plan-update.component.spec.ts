import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSummaryQuotesPlanUpdateComponent } from './show-summary-quotes-plan-update.component';

describe('ShowSummaryQuotesPlanUpdateComponent', () => {
  let component: ShowSummaryQuotesPlanUpdateComponent;
  let fixture: ComponentFixture<ShowSummaryQuotesPlanUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowSummaryQuotesPlanUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSummaryQuotesPlanUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
