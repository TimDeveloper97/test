import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryQuotesCreateComponent } from './summary-quotes-create.component';

describe('SummaryQuotesCreateComponent', () => {
  let component: SummaryQuotesCreateComponent;
  let fixture: ComponentFixture<SummaryQuotesCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SummaryQuotesCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryQuotesCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
