import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportProblemExistCreateComponent } from './report-problem-exist-create.component';

describe('ReportProblemExistCreateComponent', () => {
  let component: ReportProblemExistCreateComponent;
  let fixture: ComponentFixture<ReportProblemExistCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportProblemExistCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportProblemExistCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
