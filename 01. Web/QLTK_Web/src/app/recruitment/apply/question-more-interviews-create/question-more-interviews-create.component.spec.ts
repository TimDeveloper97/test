import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionMoreInterviewsCreateComponent } from './question-more-interviews-create.component';

describe('QuestionMoreInterviewsCreateComponent', () => {
  let component: QuestionMoreInterviewsCreateComponent;
  let fixture: ComponentFixture<QuestionMoreInterviewsCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuestionMoreInterviewsCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionMoreInterviewsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
