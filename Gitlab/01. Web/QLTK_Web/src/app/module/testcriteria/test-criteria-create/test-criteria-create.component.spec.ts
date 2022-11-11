import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestCriteriaCreateComponent } from './test-criteria-create.component';

describe('TestCriteriaCreateComponent', () => {
  let component: TestCriteriaCreateComponent;
  let fixture: ComponentFixture<TestCriteriaCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestCriteriaCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestCriteriaCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
