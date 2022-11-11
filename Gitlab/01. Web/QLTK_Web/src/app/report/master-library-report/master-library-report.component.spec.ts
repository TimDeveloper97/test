import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterLibraryReportComponent } from './master-library-report.component';

describe('MasterLibraryReportComponent', () => {
  let component: MasterLibraryReportComponent;
  let fixture: ComponentFixture<MasterLibraryReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterLibraryReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterLibraryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
