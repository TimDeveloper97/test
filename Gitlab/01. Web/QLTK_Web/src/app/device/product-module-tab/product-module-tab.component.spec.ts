import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductModuleTabComponent } from './product-module-tab.component';

describe('ProductModuleTabComponent', () => {
  let component: ProductModuleTabComponent;
  let fixture: ComponentFixture<ProductModuleTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductModuleTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductModuleTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
