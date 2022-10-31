import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportApplicationPresentComponent } from './report-application-present.component';

describe('ReportApplicationPresentComponent', () => {
  let component: ReportApplicationPresentComponent;
  let fixture: ComponentFixture<ReportApplicationPresentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportApplicationPresentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportApplicationPresentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
