import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardTPATypeManagerComponent } from './product-standard-tpa-type-manager.component';

describe('ProductStandardTPATypeManagerComponent', () => {
  let component: ProductStandardTPATypeManagerComponent;
  let fixture: ComponentFixture<ProductStandardTPATypeManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardTPATypeManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardTPATypeManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
