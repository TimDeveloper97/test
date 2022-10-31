import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportAndKeepCreateComponent } from './export-and-keep-create.component';

describe('ExportAndKeepCreateComponent', () => {
  let component: ExportAndKeepCreateComponent;
  let fixture: ComponentFixture<ExportAndKeepCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportAndKeepCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportAndKeepCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
