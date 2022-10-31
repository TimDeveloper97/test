import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductMaterialsTabComponent } from './product-materials-tab.component';

describe('ProductMaterialsTabComponent', () => {
  let component: ProductMaterialsTabComponent;
  let fixture: ComponentFixture<ProductMaterialsTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductMaterialsTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductMaterialsTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
