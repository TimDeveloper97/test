import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardTPATypeCreateComponent } from './product-standard-tpa-type-create.component';

describe('ProductStandardTPATypeCreateComponent', () => {
  let component: ProductStandardTPATypeCreateComponent;
  let fixture: ComponentFixture<ProductStandardTPATypeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardTPATypeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardTPATypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
