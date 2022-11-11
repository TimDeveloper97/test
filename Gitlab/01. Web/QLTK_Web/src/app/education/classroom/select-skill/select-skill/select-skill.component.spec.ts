import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectSkillComponent } from './select-skill.component';

describe('SelectSkillComponent', () => {
  let component: SelectSkillComponent;
  let fixture: ComponentFixture<SelectSkillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectSkillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectSkillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
