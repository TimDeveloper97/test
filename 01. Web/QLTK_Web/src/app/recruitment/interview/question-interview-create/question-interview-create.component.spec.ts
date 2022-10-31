import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionInterviewCreateComponent } from './question-interview-create.component';

describe('QuestionInterviewCreateComponent', () => {
  let component: QuestionInterviewCreateComponent;
  let fixture: ComponentFixture<QuestionInterviewCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuestionInterviewCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionInterviewCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
