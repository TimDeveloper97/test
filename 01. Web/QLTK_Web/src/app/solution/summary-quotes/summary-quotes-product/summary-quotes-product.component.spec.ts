import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryQuotesProductComponent } from './summary-quotes-product.component';

describe('SummaryQuotesProductComponent', () => {
  let component: SummaryQuotesProductComponent;
  let fixture: ComponentFixture<SummaryQuotesProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SummaryQuotesProductComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryQuotesProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
