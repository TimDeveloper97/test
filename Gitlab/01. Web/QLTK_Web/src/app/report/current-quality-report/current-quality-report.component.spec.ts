import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentQualityReportComponent } from './current-quality-report.component';

describe('CurrentQualityReportComponent', () => {
  let component: CurrentQualityReportComponent;
  let fixture: ComponentFixture<CurrentQualityReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentQualityReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentQualityReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
