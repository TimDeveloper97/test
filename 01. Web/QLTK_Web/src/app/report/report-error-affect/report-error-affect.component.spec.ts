import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportErrorAffectComponent } from './report-error-affect.component';

describe('ReportErrorAffectComponent', () => {
  let component: ReportErrorAffectComponent;
  let fixture: ComponentFixture<ReportErrorAffectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportErrorAffectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportErrorAffectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
