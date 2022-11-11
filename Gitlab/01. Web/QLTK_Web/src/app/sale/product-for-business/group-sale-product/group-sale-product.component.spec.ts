import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupSaleProductComponent } from './group-sale-product.component';

describe('GroupSaleProductComponent', () => {
  let component: GroupSaleProductComponent;
  let fixture: ComponentFixture<GroupSaleProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupSaleProductComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupSaleProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
