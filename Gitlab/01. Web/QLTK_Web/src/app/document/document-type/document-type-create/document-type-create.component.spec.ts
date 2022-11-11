import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentTypeCreateComponent } from './document-type-create.component';

describe('DocumentTypeCreateComponent', () => {
  let component: DocumentTypeCreateComponent;
  let fixture: ComponentFixture<DocumentTypeCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentTypeCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentTypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
