import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BankAccountManageComponent } from './bank-account-manage.component';

describe('BankAccountManageComponent', () => {
  let component: BankAccountManageComponent;
  let fixture: ComponentFixture<BankAccountManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BankAccountManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BankAccountManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
