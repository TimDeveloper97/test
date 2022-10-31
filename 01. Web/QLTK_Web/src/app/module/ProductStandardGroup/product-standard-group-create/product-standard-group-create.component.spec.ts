import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardGroupCreateComponent } from './product-standard-group-create.component';

describe('ProductStandardGroupCreateComponent', () => {
  let component: ProductStandardGroupCreateComponent;
  let fixture: ComponentFixture<ProductStandardGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
