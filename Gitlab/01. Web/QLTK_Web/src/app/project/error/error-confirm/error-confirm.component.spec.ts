import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorConfirmComponent } from './error-confirm.component';

describe('ErrorConfirmComponent', () => {
  let component: ErrorConfirmComponent;
  let fixture: ComponentFixture<ErrorConfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorConfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
