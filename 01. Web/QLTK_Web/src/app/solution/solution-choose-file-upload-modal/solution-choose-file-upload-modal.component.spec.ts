import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionChooseFileUploadModalComponent } from './solution-choose-file-upload-modal.component';

describe('SolutionChooseFileUploadModalComponent', () => {
  let component: SolutionChooseFileUploadModalComponent;
  let fixture: ComponentFixture<SolutionChooseFileUploadModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolutionChooseFileUploadModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionChooseFileUploadModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
