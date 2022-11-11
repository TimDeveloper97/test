import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeSkillComponent } from './practice-skill.component';

describe('PracticeSkillComponent', () => {
  let component: PracticeSkillComponent;
  let fixture: ComponentFixture<PracticeSkillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeSkillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeSkillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
