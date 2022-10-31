import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleErrorTabComponent } from './module-error-tab.component';

describe('ModuleErrorTabComponent', () => {
  let component: ModuleErrorTabComponent;
  let fixture: ComponentFixture<ModuleErrorTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleErrorTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleErrorTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
