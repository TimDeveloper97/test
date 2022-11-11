import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TiviReportErrorDepartmentComponent } from './tivi-report-error-department.component';

describe('TiviReportErrorDepartmentComponent', () => {
  let component: TiviReportErrorDepartmentComponent;
  let fixture: ComponentFixture<TiviReportErrorDepartmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TiviReportErrorDepartmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TiviReportErrorDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
