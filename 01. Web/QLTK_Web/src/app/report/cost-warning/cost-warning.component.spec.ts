import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CostWarningComponent } from './cost-warning.component';

describe('CostWarningComponent', () => {
  let component: CostWarningComponent;
  let fixture: ComponentFixture<CostWarningComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CostWarningComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CostWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
