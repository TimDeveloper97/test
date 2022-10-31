import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardEmployeeComponent } from './dashboard-employee.component';

describe('DashboardEmployeeComponent', () => {
  let component: DashboardEmployeeComponent;
  let fixture: ComponentFixture<DashboardEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
