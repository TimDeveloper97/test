import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillGroupManageComponent } from './skill-group-manage.component';

describe('SkillGroupManageComponent', () => {
  let component: SkillGroupManageComponent;
  let fixture: ComponentFixture<SkillGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
