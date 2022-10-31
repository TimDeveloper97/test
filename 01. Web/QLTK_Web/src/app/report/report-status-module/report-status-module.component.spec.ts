import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportStatusModuleComponent } from './report-status-module.component';

describe('ReportStatusModuleComponent', () => {
  let component: ReportStatusModuleComponent;
  let fixture: ComponentFixture<ReportStatusModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportStatusModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportStatusModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
