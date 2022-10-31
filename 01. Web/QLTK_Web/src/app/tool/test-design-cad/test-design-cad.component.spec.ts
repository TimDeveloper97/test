import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestDesignCadComponent } from './test-design-cad.component';

describe('TestDesignCadComponent', () => {
  let component: TestDesignCadComponent;
  let fixture: ComponentFixture<TestDesignCadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestDesignCadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestDesignCadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
