import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestStandardSuppliesComponent } from './test-standard-supplies.component';

describe('TestStandardSuppliesComponent', () => {
  let component: TestStandardSuppliesComponent;
  let fixture: ComponentFixture<TestStandardSuppliesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestStandardSuppliesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestStandardSuppliesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
