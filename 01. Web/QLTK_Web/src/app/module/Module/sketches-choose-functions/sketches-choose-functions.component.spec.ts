import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SketchesChooseFunctionsComponent } from './sketches-choose-functions.component';

describe('SketchesChooseFunctionsComponent', () => {
  let component: SketchesChooseFunctionsComponent;
  let fixture: ComponentFixture<SketchesChooseFunctionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SketchesChooseFunctionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SketchesChooseFunctionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
