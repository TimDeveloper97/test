import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionChooseFolderUploadModalComponent } from './solution-choose-folder-upload-modal.component';

describe('SolutionChooseFolderUploadModalComponent', () => {
  let component: SolutionChooseFolderUploadModalComponent;
  let fixture: ComponentFixture<SolutionChooseFolderUploadModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolutionChooseFolderUploadModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionChooseFolderUploadModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
