import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleGroupStageTabComponent } from './module-group-stage-tab.component';

describe('ModuleGroupStageTabComponent', () => {
  let component: ModuleGroupStageTabComponent;
  let fixture: ComponentFixture<ModuleGroupStageTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleGroupStageTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleGroupStageTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
