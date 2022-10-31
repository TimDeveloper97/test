import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectWorkPlaceComponent } from './select-work-place.component';

describe('SelectWorkPlaceComponent', () => {
  let component: SelectWorkPlaceComponent;
  let fixture: ComponentFixture<SelectWorkPlaceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectWorkPlaceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectWorkPlaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
