import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductErrorTabComponent } from './product-error-tab.component';

describe('ProductErrorTabComponent', () => {
  let component: ProductErrorTabComponent;
  let fixture: ComponentFixture<ProductErrorTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductErrorTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductErrorTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
