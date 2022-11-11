import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogWorkTimeComponent } from './log-work-time.component';

describe('LogWorkTimeComponent', () => {
  let component: LogWorkTimeComponent;
  let fixture: ComponentFixture<LogWorkTimeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LogWorkTimeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LogWorkTimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
