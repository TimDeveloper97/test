import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryQuotesManageComponent } from './summary-quotes-manage.component';

describe('SummaryQuotesManageComponent', () => {
  let component: SummaryQuotesManageComponent;
  let fixture: ComponentFixture<SummaryQuotesManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SummaryQuotesManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SummaryQuotesManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
