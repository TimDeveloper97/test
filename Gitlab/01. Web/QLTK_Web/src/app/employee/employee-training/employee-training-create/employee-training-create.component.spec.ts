import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeTrainingCreateComponent } from './employee-training-create.component';

describe('EmployeeTrainingCreateComponent', () => {
  let component: EmployeeTrainingCreateComponent;
  let fixture: ComponentFixture<EmployeeTrainingCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeTrainingCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeTrainingCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
