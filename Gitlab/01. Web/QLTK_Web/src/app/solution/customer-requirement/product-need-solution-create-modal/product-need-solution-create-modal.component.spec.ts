import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductNeedSolutionCreateModalComponent } from './product-need-solution-create-modal.component';

describe('ProductNeedSolutionCreateModalComponent', () => {
  let component: ProductNeedSolutionCreateModalComponent;
  let fixture: ComponentFixture<ProductNeedSolutionCreateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductNeedSolutionCreateModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductNeedSolutionCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
