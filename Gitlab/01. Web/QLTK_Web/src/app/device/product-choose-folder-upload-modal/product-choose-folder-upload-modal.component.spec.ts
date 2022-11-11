import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductChooseFolderUploadModalComponent } from './product-choose-folder-upload-modal.component';

describe('ProductChooseFolderUploadModalComponent', () => {
  let component: ProductChooseFolderUploadModalComponent;
  let fixture: ComponentFixture<ProductChooseFolderUploadModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductChooseFolderUploadModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductChooseFolderUploadModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
