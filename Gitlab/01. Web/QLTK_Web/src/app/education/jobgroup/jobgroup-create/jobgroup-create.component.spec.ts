import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobgroupCreateComponent } from './jobgroup-create.component';

describe('JobgroupCreateComponent', () => {
  let component: JobgroupCreateComponent;
  let fixture: ComponentFixture<JobgroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobgroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobgroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
