import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportErrorProgressComponent } from './report-error-progress.component';

describe('ReportErrorProgressComponent', () => {
  let component: ReportErrorProgressComponent;
  let fixture: ComponentFixture<ReportErrorProgressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportErrorProgressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportErrorProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
