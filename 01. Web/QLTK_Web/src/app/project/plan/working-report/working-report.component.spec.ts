import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkingReportComponent } from './working-report.component';

describe('WorkingReportComponent', () => {
  let component: WorkingReportComponent;
  let fixture: ComponentFixture<WorkingReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkingReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkingReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
