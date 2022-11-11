import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterEmployeeComponent } from './master-employee.component';

describe('MasterEmployeeComponent', () => {
  let component: MasterEmployeeComponent;
  let fixture: ComponentFixture<MasterEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
