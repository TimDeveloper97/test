import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestDesignManagerComponent } from './test-design-manager.component';

describe('TestDesignManagerComponent', () => {
  let component: TestDesignManagerComponent;
  let fixture: ComponentFixture<TestDesignManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestDesignManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestDesignManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
