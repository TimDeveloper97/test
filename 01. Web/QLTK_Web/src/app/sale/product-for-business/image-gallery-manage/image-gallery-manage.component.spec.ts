import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageGalleryManageComponent } from './image-gallery-manage.component';

describe('ImageGalleryManageComponent', () => {
  let component: ImageGalleryManageComponent;
  let fixture: ComponentFixture<ImageGalleryManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageGalleryManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageGalleryManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
