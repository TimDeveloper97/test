import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleDesignerTabComponent } from './module-designer-tab.component';

describe('ModuleDesignerTabComponent', () => {
  let component: ModuleDesignerTabComponent;
  let fixture: ComponentFixture<ModuleDesignerTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleDesignerTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleDesignerTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
