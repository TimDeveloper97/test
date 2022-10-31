import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BankAccountCreateComponent } from './bank-account-create.component';

describe('BankAccountCreateComponent', () => {
  let component: BankAccountCreateComponent;
  let fixture: ComponentFixture<BankAccountCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BankAccountCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BankAccountCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
