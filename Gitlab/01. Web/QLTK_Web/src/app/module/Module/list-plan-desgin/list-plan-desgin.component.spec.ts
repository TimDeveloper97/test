import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListPlanDesginComponent } from './list-plan-desgin.component';

describe('ListPlanDesginComponent', () => {
  let component: ListPlanDesginComponent;
  let fixture: ComponentFixture<ListPlanDesginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListPlanDesginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListPlanDesginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
