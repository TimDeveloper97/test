import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentSearchManageComponent } from './document-search-manage.component';

describe('DocumentSearchManageComponent', () => {
  let component: DocumentSearchManageComponent;
  let fixture: ComponentFixture<DocumentSearchManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentSearchManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentSearchManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
