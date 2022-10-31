import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OTPlanTimeComponent } from './otplan-time.component';

describe('OTPlanTimeComponent', () => {
  let component: OTPlanTimeComponent;
  let fixture: ComponentFixture<OTPlanTimeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OTPlanTimeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OTPlanTimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
