import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReportSalesBusinessComponent } from './report-sales-business.component';

describe('TestReportComponent', () => {
  let component: ReportSalesBusinessComponent;
  let fixture: ComponentFixture<ReportSalesBusinessComponent>;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      declarations: [ReportSalesBusinessComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportSalesBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
