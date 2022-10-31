import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemExistHistoryComponent } from './problem-exist-history.component';

describe('ProblemExistHistoryComponent', () => {
  let component: ProblemExistHistoryComponent;
  let fixture: ComponentFixture<ProblemExistHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProblemExistHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemExistHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
