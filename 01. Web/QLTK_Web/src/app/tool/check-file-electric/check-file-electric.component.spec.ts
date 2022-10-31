import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckFileElectricComponent } from './check-file-electric.component';

describe('CheckFileElectricComponent', () => {
  let component: CheckFileElectricComponent;
  let fixture: ComponentFixture<CheckFileElectricComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckFileElectricComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckFileElectricComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
