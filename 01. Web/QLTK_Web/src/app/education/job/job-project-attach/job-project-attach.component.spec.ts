import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobProjectAttachComponent } from './job-project-attach.component';

describe('JobProjectAttachComponent', () => {
  let component: JobProjectAttachComponent;
  let fixture: ComponentFixture<JobProjectAttachComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JobProjectAttachComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JobProjectAttachComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
