import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FuturePersonnelForecastComponent } from './future-personnel-forecast.component';

describe('FuturePersonnelForecastComponent', () => {
  let component: FuturePersonnelForecastComponent;
  let fixture: ComponentFixture<FuturePersonnelForecastComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FuturePersonnelForecastComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FuturePersonnelForecastComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
