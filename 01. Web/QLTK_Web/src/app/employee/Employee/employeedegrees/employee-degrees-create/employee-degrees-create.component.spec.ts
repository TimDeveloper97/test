import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDegreesCreateComponent } from './employee-degrees-create.component';

describe('EmployeeDegreesCreateComponent', () => {
  let component: EmployeeDegreesCreateComponent;
  let fixture: ComponentFixture<EmployeeDegreesCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDegreesCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDegreesCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
