import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentTypeManageComponent } from './document-type-manage.component';

describe('DocumentTypeManageComponent', () => {
  let component: DocumentTypeManageComponent;
  let fixture: ComponentFixture<DocumentTypeManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentTypeManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentTypeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
