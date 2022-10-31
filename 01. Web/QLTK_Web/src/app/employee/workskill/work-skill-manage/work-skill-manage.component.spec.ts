import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkSkillManageComponent } from './work-skill-manage.component';

describe('WorkSkillManageComponent', () => {
  let component: WorkSkillManageComponent;
  let fixture: ComponentFixture<WorkSkillManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkSkillManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkSkillManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
