import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSelectWorkSkillComponent } from './show-select-work-skill.component';

describe('ShowSelectWorkSkillComponent', () => {
  let component: ShowSelectWorkSkillComponent;
  let fixture: ComponentFixture<ShowSelectWorkSkillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowSelectWorkSkillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSelectWorkSkillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
