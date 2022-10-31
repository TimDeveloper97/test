import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardTpaCreateComponent } from './product-standard-tpa-create.component';

describe('ProductStandardTpaCreateComponent', () => {
  let component: ProductStandardTpaCreateComponent;
  let fixture: ComponentFixture<ProductStandardTpaCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardTpaCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardTpaCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
