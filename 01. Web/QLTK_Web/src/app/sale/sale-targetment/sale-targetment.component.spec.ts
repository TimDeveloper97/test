import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleTargetmentComponent } from './sale-targetment.component';

describe('SaleTargetmentComponent', () => {
  let component: SaleTargetmentComponent;
  let fixture: ComponentFixture<SaleTargetmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SaleTargetmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleTargetmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
