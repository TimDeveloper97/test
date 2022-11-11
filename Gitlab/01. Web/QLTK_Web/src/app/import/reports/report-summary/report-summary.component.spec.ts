import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportOngoingComponent } from './report-ongoing.component';

describe('ReportOngoingComponent', () => {
  let component: ReportOngoingComponent;
  let fixture: ComponentFixture<ReportOngoingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportOngoingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportOngoingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
