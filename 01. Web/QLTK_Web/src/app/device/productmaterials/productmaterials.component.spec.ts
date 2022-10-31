import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductmaterialsComponent } from './productmaterials.component';

describe('ProductmaterialsComponent', () => {
  let component: ProductmaterialsComponent;
  let fixture: ComponentFixture<ProductmaterialsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductmaterialsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductmaterialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
