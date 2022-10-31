import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductTabDocumentAttachComponent } from './product-tab-document-attach.component';

describe('ProductTabDocumentAttachComponent', () => {
  let component: ProductTabDocumentAttachComponent;
  let fixture: ComponentFixture<ProductTabDocumentAttachComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductTabDocumentAttachComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductTabDocumentAttachComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
