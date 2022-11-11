import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportAndKeepHistoryComponent } from './export-and-keep-history.component';

describe('ExportAndKeepHistoryComponent', () => {
  let component: ExportAndKeepHistoryComponent;
  let fixture: ComponentFixture<ExportAndKeepHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExportAndKeepHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportAndKeepHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
