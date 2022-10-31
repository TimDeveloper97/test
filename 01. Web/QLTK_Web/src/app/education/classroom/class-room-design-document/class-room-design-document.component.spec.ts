import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassRoomDesignDocumentComponent } from './class-room-design-document.component';

describe('ClassRoomDesignDocumentComponent', () => {
  let component: ClassRoomDesignDocumentComponent;
  let fixture: ComponentFixture<ClassRoomDesignDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassRoomDesignDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassRoomDesignDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
