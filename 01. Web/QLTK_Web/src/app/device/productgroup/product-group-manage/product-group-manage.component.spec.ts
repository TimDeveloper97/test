import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductGroupManageComponent } from './product-group-manage.component';

describe('ProductGroupManageComponent', () => {
  let component: ProductGroupManageComponent;
  let fixture: ComponentFixture<ProductGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
