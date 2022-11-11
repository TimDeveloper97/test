import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillGroupCreateComponent } from './skill-group-create.component';

describe('SkillGroupCreateComponent', () => {
  let component: SkillGroupCreateComponent;
  let fixture: ComponentFixture<SkillGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
