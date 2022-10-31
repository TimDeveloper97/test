import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurveyMaterialCreateComponent } from './survey-material-create.component';

describe('SurveyMaterialCreateComponent', () => {
  let component: SurveyMaterialCreateComponent;
  let fixture: ComponentFixture<SurveyMaterialCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SurveyMaterialCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SurveyMaterialCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
