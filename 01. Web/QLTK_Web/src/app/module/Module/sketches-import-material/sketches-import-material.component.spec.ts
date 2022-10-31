import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SketchesImportMaterialComponent } from './sketches-import-material.component';

describe('SketchesImportMaterialComponent', () => {
  let component: SketchesImportMaterialComponent;
  let fixture: ComponentFixture<SketchesImportMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SketchesImportMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SketchesImportMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
