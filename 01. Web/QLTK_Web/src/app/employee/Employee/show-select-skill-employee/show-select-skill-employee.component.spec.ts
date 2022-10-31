import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSelectSkillEmployeeComponents } from './show-select-skill-employee.component';

describe('ShowSelectSkillEmployeeComponents', () => {
  let component: ShowSelectSkillEmployeeComponents;
  let fixture: ComponentFixture<ShowSelectSkillEmployeeComponents>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowSelectSkillEmployeeComponents ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSelectSkillEmployeeComponents);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
