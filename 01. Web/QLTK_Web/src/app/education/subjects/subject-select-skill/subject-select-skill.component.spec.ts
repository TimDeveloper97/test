import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectSelectSkillComponent } from './subject-select-skill.component';

describe('SubjectSelectSkillComponent', () => {
  let component: SubjectSelectSkillComponent;
  let fixture: ComponentFixture<SubjectSelectSkillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectSelectSkillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectSelectSkillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
