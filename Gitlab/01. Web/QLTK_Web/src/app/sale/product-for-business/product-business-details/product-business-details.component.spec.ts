import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductBusinessDetailsComponent } from './product-business-details.component';

describe('ProductBusinessDetailsComponent', () => {
  let component: ProductBusinessDetailsComponent;
  let fixture: ComponentFixture<ProductBusinessDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductBusinessDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductBusinessDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
