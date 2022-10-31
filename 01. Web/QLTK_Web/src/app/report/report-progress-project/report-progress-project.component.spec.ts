import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportProgressProjectComponent } from './report-progress-project.component';

describe('ReportProgressProjectComponent', () => {
  let component: ReportProgressProjectComponent;
  let fixture: ComponentFixture<ReportProgressProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportProgressProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportProgressProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
