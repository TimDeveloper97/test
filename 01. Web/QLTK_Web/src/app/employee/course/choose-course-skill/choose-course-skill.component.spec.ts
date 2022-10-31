import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseCourseSkillComponent } from './choose-course-skill.component';

describe('ChooseCourseSkillComponent', () => {
  let component: ChooseCourseSkillComponent;
  let fixture: ComponentFixture<ChooseCourseSkillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseCourseSkillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseCourseSkillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
