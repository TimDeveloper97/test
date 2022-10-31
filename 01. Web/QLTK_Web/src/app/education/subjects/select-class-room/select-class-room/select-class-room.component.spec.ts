import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectClassRoomComponent } from './select-class-room.component';

describe('SelectClassRoomComponent', () => {
  let component: SelectClassRoomComponent;
  let fixture: ComponentFixture<SelectClassRoomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectClassRoomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectClassRoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
