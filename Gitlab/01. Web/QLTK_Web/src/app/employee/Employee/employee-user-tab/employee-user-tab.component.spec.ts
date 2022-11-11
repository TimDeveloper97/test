import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeUserTabComponent } from './employee-user-tab.component';

describe('EmployeeUserTabComponent', () => {
  let component: EmployeeUserTabComponent;
  let fixture: ComponentFixture<EmployeeUserTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeUserTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeUserTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
