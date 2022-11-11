import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestSoftHardCadComponent } from './test-soft-hard-cad.component';

describe('TestSoftHardCadComponent', () => {
  let component: TestSoftHardCadComponent;
  let fixture: ComponentFixture<TestSoftHardCadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestSoftHardCadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestSoftHardCadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
