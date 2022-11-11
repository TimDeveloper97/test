import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardTpaFileViewComponent } from './product-standard-tpa-file-view.component';

describe('ProductStandardTpaFileViewComponent', () => {
  let component: ProductStandardTpaFileViewComponent;
  let fixture: ComponentFixture<ProductStandardTpaFileViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardTpaFileViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardTpaFileViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
