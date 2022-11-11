import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TiviReportErrorComponent } from './tivi-report-error.component';

describe('TiviReportErrorComponent', () => {
  let component: TiviReportErrorComponent;
  let fixture: ComponentFixture<TiviReportErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TiviReportErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TiviReportErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
