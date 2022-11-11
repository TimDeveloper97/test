import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemExistHistoryChangePlanComponent } from './problem-exist-history-change-plan.component';

describe('ProblemExistHistoryChangePlanComponent', () => {
  let component: ProblemExistHistoryChangePlanComponent;
  let fixture: ComponentFixture<ProblemExistHistoryChangePlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProblemExistHistoryChangePlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemExistHistoryChangePlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
