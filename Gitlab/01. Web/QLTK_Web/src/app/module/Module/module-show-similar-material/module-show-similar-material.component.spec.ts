import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleShowSimilarMaterialComponent } from './module-show-similar-material.component';

describe('ModuleShowSimilarMaterialComponent', () => {
  let component: ModuleShowSimilarMaterialComponent;
  let fixture: ComponentFixture<ModuleShowSimilarMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleShowSimilarMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleShowSimilarMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
