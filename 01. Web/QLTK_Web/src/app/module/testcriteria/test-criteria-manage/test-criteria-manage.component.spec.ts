import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestCriteriaManageComponent } from './test-criteria-manage.component';

describe('TestCriteriaManageComponent', () => {
  let component: TestCriteriaManageComponent;
  let fixture: ComponentFixture<TestCriteriaManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestCriteriaManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestCriteriaManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
