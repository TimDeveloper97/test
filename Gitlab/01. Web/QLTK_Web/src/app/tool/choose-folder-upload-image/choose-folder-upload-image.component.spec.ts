import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseFolderUploadImageComponent } from './choose-folder-upload-image.component';

describe('ChooseFolderUploadImageComponent', () => {
  let component: ChooseFolderUploadImageComponent;
  let fixture: ComponentFixture<ChooseFolderUploadImageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseFolderUploadImageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseFolderUploadImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
