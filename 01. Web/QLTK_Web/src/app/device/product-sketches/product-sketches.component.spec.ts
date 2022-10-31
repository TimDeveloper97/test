import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductSketchesComponent } from './product-sketches.component';

describe('ProductSketchesComponent', () => {
  let component: ProductSketchesComponent;
  let fixture: ComponentFixture<ProductSketchesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductSketchesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductSketchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
