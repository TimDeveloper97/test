import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EducationProgramCreateComponent } from './education-program-create.component';

describe('EducationProgramCreateComponent', () => {
  let component: EducationProgramCreateComponent;
  let fixture: ComponentFixture<EducationProgramCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EducationProgramCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EducationProgramCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
