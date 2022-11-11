import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseCourseEmployeeComponent } from './choose-course-employee.component';

describe('ChooseCourseEmployeeComponent', () => {
  let component: ChooseCourseEmployeeComponent;
  let fixture: ComponentFixture<ChooseCourseEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseCourseEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseCourseEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
