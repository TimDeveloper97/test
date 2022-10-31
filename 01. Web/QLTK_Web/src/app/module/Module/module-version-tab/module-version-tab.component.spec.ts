import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleVersionTabComponent } from './module-version-tab.component';

describe('ModuleVersionTabComponent', () => {
  let component: ModuleVersionTabComponent;
  let fixture: ComponentFixture<ModuleVersionTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleVersionTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleVersionTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
