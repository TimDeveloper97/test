import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleGroupChooseStageComponent } from './module-group-choose-stage.component';

describe('ModuleGroupChooseStageComponent', () => {
  let component: ModuleGroupChooseStageComponent;
  let fixture: ComponentFixture<ModuleGroupChooseStageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleGroupChooseStageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleGroupChooseStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
