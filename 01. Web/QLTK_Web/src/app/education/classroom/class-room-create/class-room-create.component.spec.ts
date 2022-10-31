import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassRoomCreateComponent } from './class-room-create.component';

describe('ClassRoomCreateComponent', () => {
  let component: ClassRoomCreateComponent;
  let fixture: ComponentFixture<ClassRoomCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassRoomCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassRoomCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
