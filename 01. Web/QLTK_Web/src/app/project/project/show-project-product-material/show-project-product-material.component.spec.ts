import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectProductMaterialComponent } from './show-project-product-material.component';

describe('ShowProjectProductMaterialComponent', () => {
  let component: ShowProjectProductMaterialComponent;
  let fixture: ComponentFixture<ShowProjectProductMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectProductMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectProductMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
