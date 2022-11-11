import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportStatusProductComponent } from './report-status-product.component';

describe('ReportStatusProductComponent', () => {
  let component: ReportStatusProductComponent;
  let fixture: ComponentFixture<ReportStatusProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportStatusProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportStatusProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
