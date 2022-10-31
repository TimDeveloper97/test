import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentFileTabComponent } from './document-file-tab.component';

describe('DocumentFileTabComponent', () => {
  let component: DocumentFileTabComponent;
  let fixture: ComponentFixture<DocumentFileTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentFileTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentFileTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
