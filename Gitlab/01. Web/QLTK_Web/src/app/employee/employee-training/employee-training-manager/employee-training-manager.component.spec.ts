import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeTrainingManagerComponent } from './employee-training-manager.component';

describe('EmployeeTrainingManagerComponent', () => {
  let component: EmployeeTrainingManagerComponent;
  let fixture: ComponentFixture<EmployeeTrainingManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeTrainingManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeTrainingManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
