import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardsCreateComponent } from './product-standards-create.component';

describe('ProductStandardsCreateComponent', () => {
  let component: ProductStandardsCreateComponent;
  let fixture: ComponentFixture<ProductStandardsCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardsCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
