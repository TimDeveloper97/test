import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForecastProjectsComponent } from './forecast-projects.component';

describe('ForecastProjectsComponent', () => {
  let component: ForecastProjectsComponent;
  let fixture: ComponentFixture<ForecastProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForecastProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForecastProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
