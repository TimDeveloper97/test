import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportProblemExistManageComponent } from './report-problem-exist-manage.component';

describe('ReportProblemExistManageComponent', () => {
  let component: ReportProblemExistManageComponent;
  let fixture: ComponentFixture<ReportProblemExistManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportProblemExistManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportProblemExistManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
