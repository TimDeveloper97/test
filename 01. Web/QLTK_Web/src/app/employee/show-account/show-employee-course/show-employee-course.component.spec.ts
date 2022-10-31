import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowEmployeeCourseComponent } from './show-employee-course.component';

describe('ShowEmployeeCourseComponent', () => {
  let component: ShowEmployeeCourseComponent;
  let fixture: ComponentFixture<ShowEmployeeCourseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowEmployeeCourseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowEmployeeCourseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
