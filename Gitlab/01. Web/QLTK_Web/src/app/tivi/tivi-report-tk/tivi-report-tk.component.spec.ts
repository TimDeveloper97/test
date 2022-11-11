import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TiviReportTKComponent } from './tivi-report-tk.component';

describe('TiviReportTKComponent', () => {
  let component: TiviReportTKComponent;
  let fixture: ComponentFixture<TiviReportTKComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TiviReportTKComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TiviReportTKComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
