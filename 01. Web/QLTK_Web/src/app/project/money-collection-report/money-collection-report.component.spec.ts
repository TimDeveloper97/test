import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoneyCollectionReportComponent } from './money-collection-report.component';

describe('MoneyCollectionReportComponent', () => {
  let component: MoneyCollectionReportComponent;
  let fixture: ComponentFixture<MoneyCollectionReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoneyCollectionReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoneyCollectionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
