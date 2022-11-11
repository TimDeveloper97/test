import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductAccessoriesTabComponent } from './product-accessories-tab.component';

describe('ProductAccessoriesTabComponent', () => {
  let component: ProductAccessoriesTabComponent;
  let fixture: ComponentFixture<ProductAccessoriesTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductAccessoriesTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductAccessoriesTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
