import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralTemplateComponent } from './general-template.component';

describe('GeneralTemplateComponent', () => {
  let component: GeneralTemplateComponent;
  let fixture: ComponentFixture<GeneralTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneralTemplateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneralTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
