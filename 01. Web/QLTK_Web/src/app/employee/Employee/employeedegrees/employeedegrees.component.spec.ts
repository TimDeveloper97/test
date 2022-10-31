import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeedegreesComponent } from './employeedegrees.component';

describe('EmployeedegreesComponent', () => {
  let component: EmployeedegreesComponent;
  let fixture: ComponentFixture<EmployeedegreesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeedegreesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeedegreesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
