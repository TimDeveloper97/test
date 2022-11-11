import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerTypeManageComponent } from './customer-type-manage.component';

describe('CustomerTypeManageComponent', () => {
  let component: CustomerTypeManageComponent;
  let fixture: ComponentFixture<CustomerTypeManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerTypeManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerTypeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
