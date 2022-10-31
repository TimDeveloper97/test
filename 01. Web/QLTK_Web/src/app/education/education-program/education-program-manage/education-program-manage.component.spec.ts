import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EducationProgramManageComponent } from './education-program-manage.component';

describe('EducationProgramManageComponent', () => {
  let component: EducationProgramManageComponent;
  let fixture: ComponentFixture<EducationProgramManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EducationProgramManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EducationProgramManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
