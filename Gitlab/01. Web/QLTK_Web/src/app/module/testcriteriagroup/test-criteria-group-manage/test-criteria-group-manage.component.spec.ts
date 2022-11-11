import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestCriteriaGroupManageComponent } from './test-criteria-group-manage.component';

describe('TestCriteriaGroupManageComponent', () => {
  let component: TestCriteriaGroupManageComponent;
  let fixture: ComponentFixture<TestCriteriaGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestCriteriaGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestCriteriaGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
