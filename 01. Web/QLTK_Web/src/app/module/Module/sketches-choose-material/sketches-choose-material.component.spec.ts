import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SketchesChooseMaterialComponent } from './sketches-choose-material.component';

describe('SketchesChooseMaterialComponent', () => {
  let component: SketchesChooseMaterialComponent;
  let fixture: ComponentFixture<SketchesChooseMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SketchesChooseMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SketchesChooseMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
