import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassRoomChooseFolderUploadModalComponent } from './class-room-choose-folder-upload-modal.component';

describe('ClassRoomChooseFolderUploadModalComponent', () => {
  let component: ClassRoomChooseFolderUploadModalComponent;
  let fixture: ComponentFixture<ClassRoomChooseFolderUploadModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassRoomChooseFolderUploadModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassRoomChooseFolderUploadModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
