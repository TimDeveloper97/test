import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSelectProductComponent } from './show-select-product.component';

describe('ShowSelectProductComponent', () => {
  let component: ShowSelectProductComponent;
  let fixture: ComponentFixture<ShowSelectProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowSelectProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSelectProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
