import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkTimeManageComponent } from './work-time-manage.component';

describe('WorkTimeManageComponent', () => {
  let component: WorkTimeManageComponent;
  let fixture: ComponentFixture<WorkTimeManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkTimeManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkTimeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
