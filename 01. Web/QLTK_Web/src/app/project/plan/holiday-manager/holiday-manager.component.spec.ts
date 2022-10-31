import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidayManagerComponent } from './holiday-manager.component';

describe('HolidayManagerComponent', () => {
  let component: HolidayManagerComponent;
  let fixture: ComponentFixture<HolidayManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidayManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidayManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
