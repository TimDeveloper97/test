import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewPlanDetailComponent } from './view-plan-detail.component';

describe('ViewPlanDetailComponent', () => {
  let component: ViewPlanDetailComponent;
  let fixture: ComponentFixture<ViewPlanDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewPlanDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewPlanDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
