import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductMaterialsChooseTabComponent } from './product-materials-choose-tab.component';

describe('ProductMaterialsChooseTabComponent', () => {
  let component: ProductMaterialsChooseTabComponent;
  let fixture: ComponentFixture<ProductMaterialsChooseTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductMaterialsChooseTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductMaterialsChooseTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
