import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkTimeCreateComponent } from './work-time-create.component';

describe('WorkTimeCreateComponent', () => {
  let component: WorkTimeCreateComponent;
  let fixture: ComponentFixture<WorkTimeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkTimeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkTimeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
