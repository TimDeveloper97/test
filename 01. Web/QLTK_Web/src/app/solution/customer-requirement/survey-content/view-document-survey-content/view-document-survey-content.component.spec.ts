import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDocumentSurveyContentComponent } from './view-document-survey-content.component';

describe('ViewDocumentSurveyContentComponent', () => {
  let component: ViewDocumentSurveyContentComponent;
  let fixture: ComponentFixture<ViewDocumentSurveyContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewDocumentSurveyContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewDocumentSurveyContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
