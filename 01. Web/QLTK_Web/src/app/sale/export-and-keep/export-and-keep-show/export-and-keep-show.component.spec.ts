import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportAndKeepShowComponent } from './export-and-keep-show.component';

describe('ExportAndKeepShowComponent', () => {
  let component: ExportAndKeepShowComponent;
  let fixture: ComponentFixture<ExportAndKeepShowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExportAndKeepShowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportAndKeepShowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
