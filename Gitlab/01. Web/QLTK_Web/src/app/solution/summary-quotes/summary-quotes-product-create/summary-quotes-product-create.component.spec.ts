import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryQuotesProductCreateComponent } from './summary-quotes-product-create.component';

describe('SummaryQuotesProductCreateComponent', () => {
  let component: SummaryQuotesProductCreateComponent;
  let fixture: ComponentFixture<SummaryQuotesProductCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SummaryQuotesProductCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryQuotesProductCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
