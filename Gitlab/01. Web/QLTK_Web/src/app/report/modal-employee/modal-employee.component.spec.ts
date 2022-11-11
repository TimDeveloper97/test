import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalEmployeeComponent } from './modal-employee.component';

describe('SelectProjectComponent', () => {
  let component: ModalEmployeeComponent;
  let fixture: ComponentFixture<ModalEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ModalEmployeeComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
