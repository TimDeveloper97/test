import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardTpaViewComponent } from './product-standard-tpa-view.component';

describe('ProductStandardTpaViewComponent', () => {
  let component: ProductStandardTpaViewComponent;
  let fixture: ComponentFixture<ProductStandardTpaViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardTpaViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardTpaViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
