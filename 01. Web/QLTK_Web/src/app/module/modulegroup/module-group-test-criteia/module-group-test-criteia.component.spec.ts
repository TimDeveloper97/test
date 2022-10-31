import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleGroupTestCriteiaComponent } from './module-group-test-criteia.component';

describe('ModuleGroupTestCriteiaComponent', () => {
  let component: ModuleGroupTestCriteiaComponent;
  let fixture: ComponentFixture<ModuleGroupTestCriteiaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleGroupTestCriteiaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleGroupTestCriteiaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
