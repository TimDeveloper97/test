import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportQualityPresentComponent } from './report-quality-present.component';

describe('ReportQualityPresentComponent', () => {
  let component: ReportQualityPresentComponent;
  let fixture: ComponentFixture<ReportQualityPresentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportQualityPresentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportQualityPresentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
