import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleChooseTestCriteiaComponent } from './module-choose-test-criteia.component';

describe('ModuleChooseTestCriteiaComponent', () => {
  let component: ModuleChooseTestCriteiaComponent;
  let fixture: ComponentFixture<ModuleChooseTestCriteiaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleChooseTestCriteiaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleChooseTestCriteiaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
