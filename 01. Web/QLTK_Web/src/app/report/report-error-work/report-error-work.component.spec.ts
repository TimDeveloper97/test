import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportErrorWorkComponent } from './report-error-work.component';

describe('ReportErrorWorkComponent', () => {
  let component: ReportErrorWorkComponent;
  let fixture: ComponentFixture<ReportErrorWorkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportErrorWorkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportErrorWorkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
