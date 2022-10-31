import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryQuotesUpdateComponent } from './summary-quotes-update.component';

describe('SummaryQuotesUpdateComponent', () => {
  let component: SummaryQuotesUpdateComponent;
  let fixture: ComponentFixture<SummaryQuotesUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SummaryQuotesUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryQuotesUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
