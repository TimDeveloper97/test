import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardTpaFileComponent } from './product-standard-tpa-file.component';

describe('ProductStandardTpaFileComponent', () => {
  let component: ProductStandardTpaFileComponent;
  let fixture: ComponentFixture<ProductStandardTpaFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardTpaFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardTpaFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
