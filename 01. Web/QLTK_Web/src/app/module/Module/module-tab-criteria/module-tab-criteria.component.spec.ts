import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleTabCriteriaComponent } from './module-tab-criteria.component';

describe('ModuleTabCriteriaComponent', () => {
  let component: ModuleTabCriteriaComponent;
  let fixture: ComponentFixture<ModuleTabCriteriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleTabCriteriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleTabCriteriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
