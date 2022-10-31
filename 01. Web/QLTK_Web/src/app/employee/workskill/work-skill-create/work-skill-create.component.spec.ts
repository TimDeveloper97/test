import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkSkillCreateComponent } from './work-skill-create.component';

describe('WorkSkillCreateComponent', () => {
  let component: WorkSkillCreateComponent;
  let fixture: ComponentFixture<WorkSkillCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkSkillCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkSkillCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
