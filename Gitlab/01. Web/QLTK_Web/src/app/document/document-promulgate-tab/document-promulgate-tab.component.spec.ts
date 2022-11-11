import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentPromulgateTabComponent } from './document-promulgate-tab.component';

describe('DocumentPromulgateTabComponent', () => {
  let component: DocumentPromulgateTabComponent;
  let fixture: ComponentFixture<DocumentPromulgateTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentPromulgateTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentPromulgateTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
