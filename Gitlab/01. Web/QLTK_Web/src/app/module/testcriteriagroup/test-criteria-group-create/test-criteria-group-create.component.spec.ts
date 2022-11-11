import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestCriteriaGroupCreateComponent } from './test-criteria-group-create.component';

describe('TestCriteriaGroupCreateComponent', () => {
  let component: TestCriteriaGroupCreateComponent;
  let fixture: ComponentFixture<TestCriteriaGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestCriteriaGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestCriteriaGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
