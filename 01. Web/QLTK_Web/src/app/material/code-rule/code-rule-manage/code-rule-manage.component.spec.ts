import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeRuleManageComponent } from './code-rule-manage.component';

describe('CodeRuleManageComponent', () => {
  let component: CodeRuleManageComponent;
  let fixture: ComponentFixture<CodeRuleManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CodeRuleManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeRuleManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
