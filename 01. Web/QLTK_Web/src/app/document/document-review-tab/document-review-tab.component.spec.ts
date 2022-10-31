import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentReviewTabComponent } from './document-review-tab.component';

describe('DocumentReviewTabComponent', () => {
  let component: DocumentReviewTabComponent;
  let fixture: ComponentFixture<DocumentReviewTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentReviewTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentReviewTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
