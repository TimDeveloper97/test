import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassRoomToolComponent } from './class-room-tool.component';

describe('ClassRoomToolComponent', () => {
  let component: ClassRoomToolComponent;
  let fixture: ComponentFixture<ClassRoomToolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassRoomToolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassRoomToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
