import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductNeedPriceCreateModalComponent } from './product-need-price-create-modal.component';

describe('ProductNeedPriceCreateModalComponent', () => {
  let component: ProductNeedPriceCreateModalComponent;
  let fixture: ComponentFixture<ProductNeedPriceCreateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductNeedPriceCreateModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductNeedPriceCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
