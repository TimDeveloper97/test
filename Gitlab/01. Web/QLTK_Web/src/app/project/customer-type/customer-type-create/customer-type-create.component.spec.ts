import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerTypeCreateComponent } from './customer-type-create.component';

describe('CustomerTypeCreateComponent', () => {
  let component: CustomerTypeCreateComponent;
  let fixture: ComponentFixture<CustomerTypeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerTypeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerTypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
