import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectJobComponent } from './select-job.component';

describe('SelectJobComponent', () => {
  let component: SelectJobComponent;
  let fixture: ComponentFixture<SelectJobComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectJobComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectJobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
