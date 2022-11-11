import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestDesignPopupReportComponent } from './test-design-popup-report.component';

describe('TestDesignPopupReportComponent', () => {
  let component: TestDesignPopupReportComponent;
  let fixture: ComponentFixture<TestDesignPopupReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestDesignPopupReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestDesignPopupReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
