import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportAndKeepDetailComponent } from './export-and-keep-detail.component';

describe('ExportAndKeepDetailComponent', () => {
  let component: ExportAndKeepDetailComponent;
  let fixture: ComponentFixture<ExportAndKeepDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportAndKeepDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportAndKeepDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
