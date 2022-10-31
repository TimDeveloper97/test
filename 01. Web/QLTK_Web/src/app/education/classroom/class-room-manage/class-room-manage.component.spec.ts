import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassRoomManageComponent } from './class-room-manage.component';

describe('ClassRoomManageComponent', () => {
  let component: ClassRoomManageComponent;
  let fixture: ComponentFixture<ClassRoomManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassRoomManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassRoomManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
