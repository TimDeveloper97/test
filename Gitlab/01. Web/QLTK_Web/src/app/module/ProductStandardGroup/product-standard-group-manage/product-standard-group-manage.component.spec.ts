import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductStandardGroupManageComponent } from './product-standard-group-manage.component';

describe('ProductStandardGroupManageComponent', () => {
  let component: ProductStandardGroupManageComponent;
  let fixture: ComponentFixture<ProductStandardGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductStandardGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductStandardGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
