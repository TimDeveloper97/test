import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeCourseComponent } from './employee-course.component';

describe('EmployeeCourseComponent', () => {
  let component: EmployeeCourseComponent;
  let fixture: ComponentFixture<EmployeeCourseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeCourseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeCourseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
