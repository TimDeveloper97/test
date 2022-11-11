import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSummaryQuotesPlanCreateComponent } from './show-summary-quotes-plan-create.component';

describe('ShowSummaryQuotesPlanCreateComponent', () => {
  let component: ShowSummaryQuotesPlanCreateComponent;
  let fixture: ComponentFixture<ShowSummaryQuotesPlanCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowSummaryQuotesPlanCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSummaryQuotesPlanCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
