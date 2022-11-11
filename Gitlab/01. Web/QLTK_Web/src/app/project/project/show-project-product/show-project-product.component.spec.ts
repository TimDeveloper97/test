import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectProductComponent } from './show-project-product.component';

describe('ShowProjectProductComponent', () => {
  let component: ShowProjectProductComponent;
  let fixture: ComponentFixture<ShowProjectProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectProductComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
