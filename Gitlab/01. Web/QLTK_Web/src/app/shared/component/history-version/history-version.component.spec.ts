import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoryVersionComponent } from './history-version.component';

describe('HistoryVersionComponent', () => {
  let component: HistoryVersionComponent;
  let fixture: ComponentFixture<HistoryVersionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HistoryVersionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoryVersionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
