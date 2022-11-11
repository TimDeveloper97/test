import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseDocumentComponent } from './choose-document.component';

describe('ChooseDocumentComponent', () => {
  let component: ChooseDocumentComponent;
  let fixture: ComponentFixture<ChooseDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseDocumentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
