import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductForBusinessCreateComponent } from './product-for-business-create.component';

describe('ProductForBusinessCreateComponent', () => {
  let component: ProductForBusinessCreateComponent;
  let fixture: ComponentFixture<ProductForBusinessCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductForBusinessCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductForBusinessCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
