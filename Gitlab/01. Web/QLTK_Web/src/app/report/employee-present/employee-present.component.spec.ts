import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeePresentComponent } from './employee-present.component';

describe('EmployeePresentComponent', () => {
  let component: EmployeePresentComponent;
  let fixture: ComponentFixture<EmployeePresentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeePresentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePresentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
