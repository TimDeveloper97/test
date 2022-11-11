import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurveyContentManageComponent } from './survey-content-manage.component';

describe('SurveyContentManageComponent', () => {
  let component: SurveyContentManageComponent;
  let fixture: ComponentFixture<SurveyContentManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SurveyContentManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SurveyContentManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
