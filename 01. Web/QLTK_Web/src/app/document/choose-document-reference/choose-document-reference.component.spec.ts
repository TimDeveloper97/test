import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseDocumentReferenceComponent } from './choose-document-reference.component';

describe('ChooseDocumentReferenceComponent', () => {
  let component: ChooseDocumentReferenceComponent;
  let fixture: ComponentFixture<ChooseDocumentReferenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseDocumentReferenceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseDocumentReferenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
