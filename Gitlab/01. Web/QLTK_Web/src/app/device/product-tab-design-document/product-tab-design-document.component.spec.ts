import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductTabDesignDocumentComponent } from './product-tab-design-document.component';

describe('ProductTabDesignDocumentComponent', () => {
  let component: ProductTabDesignDocumentComponent;
  let fixture: ComponentFixture<ProductTabDesignDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductTabDesignDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductTabDesignDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
