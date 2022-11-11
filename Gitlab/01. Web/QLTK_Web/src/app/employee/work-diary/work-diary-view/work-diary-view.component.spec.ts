import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkDiaryViewComponent } from './work-diary-view.component';

describe('WorkDiaryViewComponent', () => {
  let component: WorkDiaryViewComponent;
  let fixture: ComponentFixture<WorkDiaryViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkDiaryViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkDiaryViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
