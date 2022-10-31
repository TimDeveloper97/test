import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductNeedPublicationsComponent } from './product-need-publications.component';

describe('ProductNeedPublicationsComponent', () => {
  let component: ProductNeedPublicationsComponent;
  let fixture: ComponentFixture<ProductNeedPublicationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductNeedPublicationsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductNeedPublicationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
