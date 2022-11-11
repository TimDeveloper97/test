import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleChooseDesignerTabComponent } from './module-choose-designer-tab.component';

describe('ModuleChooseDesignerTabComponent', () => {
  let component: ModuleChooseDesignerTabComponent;
  let fixture: ComponentFixture<ModuleChooseDesignerTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleChooseDesignerTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleChooseDesignerTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
