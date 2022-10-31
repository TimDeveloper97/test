import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseFolderUploadModalComponent } from './choose-folder-upload-modal.component';

describe('ChooseFolderUploadModalComponent', () => {
  let component: ChooseFolderUploadModalComponent;
  let fixture: ComponentFixture<ChooseFolderUploadModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseFolderUploadModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseFolderUploadModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
