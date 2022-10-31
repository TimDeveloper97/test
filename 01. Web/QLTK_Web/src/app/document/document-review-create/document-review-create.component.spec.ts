import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentReviewCreateComponent } from './document-review-create.component';

describe('DocumentReviewCreateComponent', () => {
  let component: DocumentReviewCreateComponent;
  let fixture: ComponentFixture<DocumentReviewCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentReviewCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentReviewCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
