import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentGroupCreateComponent } from './document-group-create.component';

describe('DocumentGroupCreateComponent', () => {
  let component: DocumentGroupCreateComponent;
  let fixture: ComponentFixture<DocumentGroupCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentGroupCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
