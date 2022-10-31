import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobPositionManageComponent } from './job-position-manage.component';

describe('JobPositionManageComponent', () => {
  let component: JobPositionManageComponent;
  let fixture: ComponentFixture<JobPositionManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobPositionManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobPositionManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
