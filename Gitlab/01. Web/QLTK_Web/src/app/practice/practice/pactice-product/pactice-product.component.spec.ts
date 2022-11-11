import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PacticeProductComponent } from './pactice-product.component';

describe('PacticeProductComponent', () => {
  let component: PacticeProductComponent;
  let fixture: ComponentFixture<PacticeProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PacticeProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PacticeProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
