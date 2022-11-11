import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentCostWarningComponent } from './current-cost-warning.component';

describe('CurrentCostWarningComponent', () => {
  let component: CurrentCostWarningComponent;
  let fixture: ComponentFixture<CurrentCostWarningComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentCostWarningComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentCostWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
