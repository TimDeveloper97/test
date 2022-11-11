import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleMaterialTabComponent } from './module-material-tab.component';

describe('ModuleMaterialTabComponent', () => {
  let component: ModuleMaterialTabComponent;
  let fixture: ComponentFixture<ModuleMaterialTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleMaterialTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleMaterialTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
