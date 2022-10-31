import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeGroupCreateComponent } from './employee-group-create.component';

describe('EmployeeGroupCreateComponent', () => {
  let component: EmployeeGroupCreateComponent;
  let fixture: ComponentFixture<EmployeeGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
