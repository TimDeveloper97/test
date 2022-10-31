import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeGroupManageComponent } from './employee-group-manage.component';

describe('EmployeeGroupManageComponent', () => {
  let component: EmployeeGroupManageComponent;
  let fixture: ComponentFixture<EmployeeGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
