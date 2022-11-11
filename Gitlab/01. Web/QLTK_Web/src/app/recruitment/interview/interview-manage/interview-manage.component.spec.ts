import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewManageComponent } from './interview-manage.component';

describe('InterviewManageComponent', () => {
  let component: InterviewManageComponent;
  let fixture: ComponentFixture<InterviewManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InterviewManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InterviewManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
