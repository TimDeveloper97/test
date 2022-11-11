import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusReportMaterialComponent } from './status-report-material.component';

describe('StatusReportMaterialComponent', () => {
  let component: StatusReportMaterialComponent;
  let fixture: ComponentFixture<StatusReportMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatusReportMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatusReportMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
