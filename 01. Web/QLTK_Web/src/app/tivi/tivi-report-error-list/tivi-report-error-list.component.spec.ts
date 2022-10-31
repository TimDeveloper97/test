import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TiviReportErrorListComponent } from './tivi-report-error-list.component';

describe('TiviReportErrorListComponent', () => {
  let component: TiviReportErrorListComponent;
  let fixture: ComponentFixture<TiviReportErrorListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TiviReportErrorListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TiviReportErrorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
