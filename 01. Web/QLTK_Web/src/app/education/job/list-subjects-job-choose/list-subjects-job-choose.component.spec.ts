import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListSubjectsJobChooseComponent } from './list-subjects-job-choose.component';

describe('ListSubjectsJobChooseComponent', () => {
  let component: ListSubjectsJobChooseComponent;
  let fixture: ComponentFixture<ListSubjectsJobChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListSubjectsJobChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListSubjectsJobChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
