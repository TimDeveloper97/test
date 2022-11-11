import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleProductstandardTabComponent } from './module-productstandard-tab.component';

describe('ModuleProductstandardTabComponent', () => {
  let component: ModuleProductstandardTabComponent;
  let fixture: ComponentFixture<ModuleProductstandardTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleProductstandardTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleProductstandardTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
