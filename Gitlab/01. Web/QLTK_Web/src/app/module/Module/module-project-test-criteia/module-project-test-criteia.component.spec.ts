import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleProjectTestCriteiaComponent } from './module-project-test-criteia.component';

describe('ModuleProjectTestCriteiaComponent', () => {
  let component: ModuleProjectTestCriteiaComponent;
  let fixture: ComponentFixture<ModuleProjectTestCriteiaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleProjectTestCriteiaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleProjectTestCriteiaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
