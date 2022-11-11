import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanHistoryViewComponent } from './plan-history-view.component';

describe('PlanHistoryViewComponent', () => {
  let component: PlanHistoryViewComponent;
  let fixture: ComponentFixture<PlanHistoryViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanHistoryViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanHistoryViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
