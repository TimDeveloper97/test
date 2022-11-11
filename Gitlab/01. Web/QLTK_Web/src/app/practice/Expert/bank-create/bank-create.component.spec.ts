import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BankCreateComponent } from './bank-create.component';

describe('BankCreateComponent', () => {
  let component: BankCreateComponent;
  let fixture: ComponentFixture<BankCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BankCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BankCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
