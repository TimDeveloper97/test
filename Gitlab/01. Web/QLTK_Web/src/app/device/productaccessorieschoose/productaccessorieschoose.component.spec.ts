import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductaccessorieschooseComponent } from './productaccessorieschoose.component';

describe('ProductaccessorieschooseComponent', () => {
  let component: ProductaccessorieschooseComponent;
  let fixture: ComponentFixture<ProductaccessorieschooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductaccessorieschooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductaccessorieschooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
