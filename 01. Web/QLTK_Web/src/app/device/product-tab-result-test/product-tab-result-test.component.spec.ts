import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductTabResultTestComponent } from './product-tab-result-test.component';

describe('ProductTabResultTestComponent', () => {
  let component: ProductTabResultTestComponent;
  let fixture: ComponentFixture<ProductTabResultTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductTabResultTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductTabResultTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
