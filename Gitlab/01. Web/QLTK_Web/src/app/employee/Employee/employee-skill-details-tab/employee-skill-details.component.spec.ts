import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeSkillDetailsComponent } from './employee-skill-details.component';

describe('EmployeeSkillDetailsComponent', () => {
  let component: EmployeeSkillDetailsComponent;
  let fixture: ComponentFixture<EmployeeSkillDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeSkillDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeSkillDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
