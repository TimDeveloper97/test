import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleMaterialSetupTabComponent } from './module-material-setup-tab.component';

describe('ModuleMaterialSetupTabComponent', () => {
  let component: ModuleMaterialSetupTabComponent;
  let fixture: ComponentFixture<ModuleMaterialSetupTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleMaterialSetupTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleMaterialSetupTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
