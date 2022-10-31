import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductUpdateContentComponent } from './product-update-content.component';

describe('ProductUpdateContentComponent', () => {
  let component: ProductUpdateContentComponent;
  let fixture: ComponentFixture<ProductUpdateContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductUpdateContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductUpdateContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
