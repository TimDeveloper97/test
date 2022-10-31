import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportProfileHistoryComponent } from './import-profile-history.component';

describe('ImportProfileHistoryComponent', () => {
  let component: ImportProfileHistoryComponent;
  let fixture: ComponentFixture<ImportProfileHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImportProfileHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportProfileHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
