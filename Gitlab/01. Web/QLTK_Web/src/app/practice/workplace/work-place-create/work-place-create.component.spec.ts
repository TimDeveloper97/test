import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkPlaceCreateComponent } from './work-place-create.component';

describe('WorkPlaceCreateComponent', () => {
  let component: WorkPlaceCreateComponent;
  let fixture: ComponentFixture<WorkPlaceCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkPlaceCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkPlaceCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
