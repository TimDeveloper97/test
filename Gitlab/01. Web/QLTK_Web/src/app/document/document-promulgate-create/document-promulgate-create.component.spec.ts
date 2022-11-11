import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentPromulgateCreateComponent } from './document-promulgate-create.component';

describe('DocumentPromulgateCreateComponent', () => {
  let component: DocumentPromulgateCreateComponent;
  let fixture: ComponentFixture<DocumentPromulgateCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentPromulgateCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentPromulgateCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
