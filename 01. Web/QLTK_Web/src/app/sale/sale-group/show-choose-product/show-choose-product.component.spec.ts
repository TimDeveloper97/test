import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseProductComponent } from './show-choose-product.component';

describe('ShowChooseProductComponent', () => {
  let component: ShowChooseProductComponent;
  let fixture: ComponentFixture<ShowChooseProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowChooseProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
