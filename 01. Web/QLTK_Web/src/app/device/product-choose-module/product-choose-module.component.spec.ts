import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductChooseModuleComponent } from './product-choose-module.component';

describe('ProductChooseModuleComponent', () => {
  let component: ProductChooseModuleComponent;
  let fixture: ComponentFixture<ProductChooseModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductChooseModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductChooseModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
