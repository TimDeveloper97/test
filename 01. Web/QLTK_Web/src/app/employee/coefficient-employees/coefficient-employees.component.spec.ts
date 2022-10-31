import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoefficientEmployeesComponent } from './coefficient-employees.component';

describe('CoefficientEmployeesComponent', () => {
  let component: CoefficientEmployeesComponent;
  let fixture: ComponentFixture<CoefficientEmployeesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoefficientEmployeesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoefficientEmployeesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
