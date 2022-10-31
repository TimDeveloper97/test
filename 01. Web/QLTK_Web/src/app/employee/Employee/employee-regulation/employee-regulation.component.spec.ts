import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeRegulationComponent } from './employee-regulation.component';

describe('EmployeeRegulationComponent', () => {
  let component: EmployeeRegulationComponent;
  let fixture: ComponentFixture<EmployeeRegulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeRegulationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeRegulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
