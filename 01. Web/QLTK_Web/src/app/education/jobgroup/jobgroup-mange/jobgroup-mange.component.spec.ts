import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobgroupMangeComponent } from './jobgroup-mange.component';

describe('JobgroupMangeComponent', () => {
  let component: JobgroupMangeComponent;
  let fixture: ComponentFixture<JobgroupMangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobgroupMangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobgroupMangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
