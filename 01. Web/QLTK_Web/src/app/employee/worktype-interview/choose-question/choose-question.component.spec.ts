import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseQuestionComponent } from './choose-question.component';

describe('ChooseQuestionComponent', () => {
  let component: ChooseQuestionComponent;
  let fixture: ComponentFixture<ChooseQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseQuestionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
