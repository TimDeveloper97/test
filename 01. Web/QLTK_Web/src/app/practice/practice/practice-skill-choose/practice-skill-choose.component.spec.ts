import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeSkillChooseComponent } from './practice-skill-choose.component';

describe('PracticeSkillChooseComponent', () => {
  let component: PracticeSkillChooseComponent;
  let fixture: ComponentFixture<PracticeSkillChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeSkillChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeSkillChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
