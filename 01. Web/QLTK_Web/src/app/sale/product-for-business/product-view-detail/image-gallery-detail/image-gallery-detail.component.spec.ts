import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageGalleryDetailComponent } from './image-gallery-detail.component';

describe('ImageGalleryDetailComponent', () => {
  let component: ImageGalleryDetailComponent;
  let fixture: ComponentFixture<ImageGalleryDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImageGalleryDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageGalleryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
