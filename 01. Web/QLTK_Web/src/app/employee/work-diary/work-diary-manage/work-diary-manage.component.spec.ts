import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkDiaryManageComponent } from './work-diary-manage.component';

describe('WorkDiaryManageComponent', () => {
  let component: WorkDiaryManageComponent;
  let fixture: ComponentFixture<WorkDiaryManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkDiaryManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkDiaryManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
