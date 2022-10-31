import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralInformationProjectComponent } from './general-information-project.component';

describe('GeneralInformationProjectComponent', () => {
  let component: GeneralInformationProjectComponent;
  let fixture: ComponentFixture<GeneralInformationProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneralInformationProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneralInformationProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
