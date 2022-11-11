import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkSkillGroupCreateComponent } from './work-skill-group-create.component';

describe('WorkSkillGroupCreateComponent', () => {
  let component: WorkSkillGroupCreateComponent;
  let fixture: ComponentFixture<WorkSkillGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkSkillGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkSkillGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
