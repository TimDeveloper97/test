import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TotalTimePerformComponent } from './total-time-perform.component';

describe('TotalTimePerformComponent', () => {
  let component: TotalTimePerformComponent;
  let fixture: ComponentFixture<TotalTimePerformComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TotalTimePerformComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TotalTimePerformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
