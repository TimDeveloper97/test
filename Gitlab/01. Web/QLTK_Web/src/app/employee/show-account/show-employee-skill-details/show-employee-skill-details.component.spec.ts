import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowEmployeeSkillDetailsComponent } from './show-employee-skill-details.component';

describe('ShowEmployeeSkillDetailsComponent', () => {
  let component: ShowEmployeeSkillDetailsComponent;
  let fixture: ComponentFixture<ShowEmployeeSkillDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowEmployeeSkillDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowEmployeeSkillDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
