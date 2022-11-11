import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurveyContentCreateComponent } from './survey-content-create.component';

describe('SurveyContentCreateComponent', () => {
  let component: SurveyContentCreateComponent;
  let fixture: ComponentFixture<SurveyContentCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SurveyContentCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SurveyContentCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
