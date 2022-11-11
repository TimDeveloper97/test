import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadImageVideoComponent } from './upload-image-video.component';

describe('UploadImageVideoComponent', () => {
  let component: UploadImageVideoComponent;
  let fixture: ComponentFixture<UploadImageVideoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadImageVideoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadImageVideoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
