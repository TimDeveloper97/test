import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardsUpdateComponent } from './product-standards-update.component';

describe('ProductStandardsUpdateComponent', () => {
  let component: ProductStandardsUpdateComponent;
  let fixture: ComponentFixture<ProductStandardsUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardsUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardsUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
