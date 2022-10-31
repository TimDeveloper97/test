import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerProjectComponent } from './customer-project.component';

describe('CustomerProjectComponent', () => {
  let component: CustomerProjectComponent;
  let fixture: ComponentFixture<CustomerProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
